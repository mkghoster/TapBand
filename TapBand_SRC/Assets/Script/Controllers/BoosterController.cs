using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public enum BoosterType
{
    AutoTap,
    ExtraTime,
    TapStrength
}

public class BoosterController : MonoBehaviour
{
    private bool isTapStrengthBoosterisActive = false;
    private bool isAutoTapActive = false;

    private float tapStrengthBoostDuration = 0;
    private float autoTapBoostDuration = 0;

    private SongController songController;
    private CurrencyController currencyController;
    private ViewController viewController;

    private BoosterDropZone boosterDropZone;
    private BoosterUI boosterUI;

    private BoosterData boosterData;
    private IapData iapData;

    private DailyEventState dailyEventState;

    private float lastAutoTap = 0;
    private float autoTapInterval;
    private TapUI tapUI;

    private bool canActivateBoosters = false;

    public event BoosterEvent OnBoosterActivated;
    public event BoosterEvent OnBoosterFinished;
    public event BoosterEvent OnBoosterStateChanged;

    private bool isPaused = false;

    public event RawTapEvent OnAutoTap;

    void Awake()
    {
        var gameData = GameData.instance;
        boosterData = gameData.BoosterData;
        iapData = gameData.IapData;


        songController = FindObjectOfType<SongController>();
        currencyController = FindObjectOfType<CurrencyController>();
        viewController = FindObjectOfType<ViewController>();

        boosterDropZone = FindObjectOfType<BoosterDropZone>();
        tapUI = FindObjectOfType<TapUI>();

        boosterUI = FindObjectOfType<BoosterUI>();

        autoTapInterval = 1f / boosterData.AutoTapBoosterTapsPerSecond;

        dailyEventState = GameState.instance.DailyEvent;
    }

    void OnEnable()
    {
        boosterDropZone.OnBoosterDropped += HandleBoosterDropped;
        songController.OnSongStarted += HandleSongStarted;
        songController.OnSongFinished += HandleSongFinished;
        viewController.OnViewChange += HandleViewChanged;
    }

    void OnDisable()
    {
        boosterDropZone.OnBoosterDropped -= HandleBoosterDropped;
        songController.OnSongStarted -= HandleSongStarted;
        songController.OnSongFinished -= HandleSongFinished;
        viewController.OnViewChange -= HandleViewChanged;
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        if (isTapStrengthBoosterisActive && tapStrengthBoostDuration <= 0)
        {
            isTapStrengthBoosterisActive = false;
            if (OnBoosterFinished != null)
            {
                OnBoosterFinished(this, new BoosterEventArgs(BoosterType.TapStrength));
            }
        }

        if (isAutoTapActive && autoTapBoostDuration <= 0)
        {
            isAutoTapActive = false;
            if (OnBoosterFinished != null)
            {
                OnBoosterFinished(this, new BoosterEventArgs(BoosterType.AutoTap));
            }
        }

        if (isAutoTapActive)
        {
            List<RawTapData> rawTaps = new List<RawTapData>();

            while (lastAutoTap < Time.time)
            {
                lastAutoTap += autoTapInterval;
                rawTaps.Add(GenerateRandomTapEventArgs());
            }

            if (rawTaps.Count > 0 && OnAutoTap != null)
            {
                OnAutoTap(this, new RawTapEventArgs(rawTaps));
            }
            autoTapBoostDuration -= Time.deltaTime;
        }
        if (isTapStrengthBoosterisActive)
        {
            tapStrengthBoostDuration -= Time.deltaTime;
        }
    }

    private RawTapData GenerateRandomTapEventArgs()
    {
        //TODO make the correct values
        int x = 200;
        int y = 500;
        Vector2 autoTapPosition = new Vector2(x, y);
        return new RawTapData(autoTapPosition, false);
    }

    private void HandleBoosterDropped(object sender, BoosterEventArgs e)
    {
        if (CanActivateBooster(e.Type))
        {
            switch (e.Type)
            {
                case BoosterType.AutoTap:
                    autoTapBoostDuration = boosterData.AutoTapBoosterDuration;
                    isAutoTapActive = true;
                    lastAutoTap = Time.time;
                    break;
                case BoosterType.TapStrength:
                    tapStrengthBoostDuration = boosterData.TapStrengthBoosterDuration;
                    isTapStrengthBoosterisActive = true;
                    break;
            }

            if (OnBoosterActivated != null)
            {
                OnBoosterActivated(this, e);
            }
        }
    }

    public float GetTapStrengthMultiplier()
    {
        if (isTapStrengthBoosterisActive)
        {
            return boosterData.TapStrengthBoosterMultiplier;
        }
        return 1f;

        //return isTapStrengthBoosterisActive ? boosterData.TapStrengthBoosterMultiplier : 1f;
    }

    public bool CanActivateBooster(BoosterType type)
    {
        if (!canActivateBoosters)
        {
            return false;
        }
        int boosterPrice = 0;
        switch (type)
        {
            case BoosterType.AutoTap:
                if (isAutoTapActive)
                {
                    return false;
                }
                boosterPrice = Mathf.FloorToInt(iapData.autoTapBoosterCost * dailyEventState.AutoTapBoosterPriceMultiplier);
                break;
            case BoosterType.ExtraTime:

                boosterPrice = Mathf.FloorToInt(iapData.extraTimeBoosterCost * dailyEventState.ExtraTimeBoosterPriceMultiplier);
                break;
            case BoosterType.TapStrength:
                if (isTapStrengthBoosterisActive)
                {
                    return false;
                }
                boosterPrice = Mathf.FloorToInt(iapData.tapStrenghtBoosterCost * dailyEventState.TapStrengthBoosterPriceMultiplier);
                break;
            default:

                throw new NotImplementedException("Implement CanActivate for this booster type!");
        }

        return currencyController.CanBuyFromToken(boosterPrice);
    }

    private void HandleSongStarted(object sender, SongEventArgs e)
    {
        canActivateBoosters = true;
        if (OnBoosterStateChanged != null)
        {
            OnBoosterStateChanged(this, new BoosterEventArgs(BoosterType.AutoTap)); // this is not exactly good, as this is a generic event, but null would be worse.
        }
    }

    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        canActivateBoosters = false;
        if (OnBoosterStateChanged != null)
        {
            OnBoosterStateChanged(this, new BoosterEventArgs(BoosterType.AutoTap)); // this is not exactly good, as this is a generic event, but null would be worse.
        }
    }

    private void HandleViewChanged(object sender, ViewChangeEventArgs e)
    {
        bool isStage = (e.NewView == ViewType.STAGE);
        boosterUI.SetUIVisible(isStage);
        isPaused = !isStage;
    }
}