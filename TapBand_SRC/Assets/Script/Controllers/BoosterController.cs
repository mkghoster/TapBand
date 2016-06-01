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

    private float boostTapStrengthUntil = 0;
    private float autoTapUntil = 0;

    private SongController songController;
    private CurrencyController currencyController;

    private BoosterDropZone boosterDropZone;

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

    void Awake()
    {
        var gameData = GameData.instance;
        boosterData = gameData.BoosterData;
        iapData = gameData.IapData;
        songController = FindObjectOfType<SongController>();
        currencyController = FindObjectOfType<CurrencyController>();
        boosterDropZone = FindObjectOfType<BoosterDropZone>();
        tapUI = (TapUI)FindObjectOfType(typeof(TapUI));

        autoTapInterval = 1f / boosterData.AutoTapBoosterTapsPerSecond;

        dailyEventState = GameState.instance.DailyEvent;
    }

    void OnEnable()
    {
        boosterDropZone.OnBoosterDropped += HandleBoosterDropped;
        songController.OnSongStarted += HandleSongStarted;
        songController.OnSongFinished += HandleSongFinished;
    }

    void OnDisable()
    {
        boosterDropZone.OnBoosterDropped -= HandleBoosterDropped;
        songController.OnSongStarted -= HandleSongStarted;
        songController.OnSongFinished -= HandleSongFinished;
    }

    void Update()
    {
        if (isTapStrengthBoosterisActive && Time.time > boostTapStrengthUntil)
        {
            isTapStrengthBoosterisActive = false;
            if (OnBoosterFinished != null)
            {
                OnBoosterFinished(this, new BoosterEventArgs(BoosterType.TapStrength));
            }
        }

        if (isAutoTapActive && Time.time > autoTapUntil)
        {
            isAutoTapActive = false;
            if (OnBoosterFinished != null)
            {
                OnBoosterFinished(this, new BoosterEventArgs(BoosterType.AutoTap));
            }
        }

        if (isAutoTapActive)
        {
            while (lastAutoTap < Time.time)
            {
                lastAutoTap += autoTapInterval;
                tapUI.AutoTap();
            }
        }
    }

    private void HandleBoosterDropped(object sender, BoosterEventArgs e)
    {
        if (CanActivateBooster(e.Type))
        {
            switch (e.Type)
            {
                case BoosterType.AutoTap:
                    autoTapUntil = Time.time + boosterData.AutoTapBoosterDuration;
                    isAutoTapActive = true;
                    lastAutoTap = Time.time;
                    break;
                case BoosterType.TapStrength:
                    boostTapStrengthUntil = Time.time + boosterData.AutoTapBoosterDuration;
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
}