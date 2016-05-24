using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoosterController : MonoBehaviour {

    enum BoosterDataType { TapStrengthBoosterMultiplier, TapStrengthBoosterDuration, ExtraTimeBoosterBonus, AutoTapBoosterInterval, AutoTapBoosterDuration };
    private float CurrentBoosterDuration;
    private float CurrentAutoTapBoosterDuration=100;
    private bool tapStrengthBoosterisActive = false;
    private bool autoTapisActive = false;
    private TapController tapController;
    private SongController songController;
    private BoosterData boosterData;
    private float autoTapBoosterTapsPerSecond;
    private double baseAutoTapRate;
    private float expectedAutoTapCount;
    private double autoTapRateSum;
    private int currentAutoTapCount = 0;

    private TapUI tapUI;

    void Start()
    {
        boosterData = GameData.instance.BoosterData;
        tapController = (TapController)FindObjectOfType(typeof(TapController));
        songController = (SongController)FindObjectOfType(typeof(SongController));
        tapUI = (TapUI)FindObjectOfType(typeof(TapUI));
    }

    void Update()
    {
        if (tapStrengthBoosterisActive)
        {
            CurrentBoosterDuration -= Time.deltaTime;
        }
        if (CurrentBoosterDuration <= 0)
        {
            tapStrengthBoosterisActive = false;
            tapController.BoosterTimeInterval(0);
        }

        if (autoTapisActive)
        {
            CurrentAutoTapBoosterDuration -= Time.deltaTime;
            if (CurrentAutoTapBoosterDuration < (double)boosterData.AutoTapBoosterDuration - autoTapRateSum && CurrentAutoTapBoosterDuration > 0)
            {             
                currentAutoTapCount++;
                expectedAutoTapCount++;
                autoTapRateSum += baseAutoTapRate;
                tapUI.AutoTap();
            }
        }

        if (CurrentAutoTapBoosterDuration <= 0 || currentAutoTapCount == expectedAutoTapCount)
        {
            CurrentAutoTapBoosterDuration = boosterData.AutoTapBoosterDuration;
            autoTapisActive = false;
            currentAutoTapCount = 0;
            autoTapRateSum = baseAutoTapRate;
        }
    }



    public void HandleBoosters(BoosterUI currentBooster)
    {
        if (currentBooster.name.Equals("TapStrengthBooster"))
        {
            tapController.BoosterMultiplier(boosterData.TapStrengthBoosterMultiplier);
            tapController.BoosterTimeInterval(boosterData.TapStrengthBoosterDuration);
            CurrentBoosterDuration = boosterData.TapStrengthBoosterDuration;
            tapStrengthBoosterisActive = true;
            StartCoroutine(SetBoosterIsActive(currentBooster));
        }
        else if (currentBooster.name.Equals("AutoTapBooster"))
        {
            CurrentAutoTapBoosterDuration = boosterData.AutoTapBoosterDuration;
            autoTapBoosterTapsPerSecond = boosterData.AutoTapBoosterTapsPerSecond;
            baseAutoTapRate = (double)1 / autoTapBoosterTapsPerSecond;
            expectedAutoTapCount = autoTapBoosterTapsPerSecond * CurrentAutoTapBoosterDuration;
            autoTapisActive = true;
            CurrentBoosterDuration = CurrentAutoTapBoosterDuration;
            StartCoroutine(SetBoosterIsActive(currentBooster));
        }
        else if (currentBooster.name.Equals("ExtraTimeBooster"))
        {

            //TODO currentsong.duration should be increased instead of decreasing the timecountdown
            songController.BossExtratime(boosterData.ExtraTimeBoosterBonus);
            CurrentBoosterDuration = boosterData.ExtraTimeBoosterBonus;
            StartCoroutine(SetBoosterIsActive(currentBooster));
        }
    }

    IEnumerator SetBoosterIsActive(BoosterUI currentBooster)
    {
        currentBooster.boosterIsActive = true;
        currentBooster.boosterIsAvailable = false;
        currentBooster.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(CurrentBoosterDuration);
        currentBooster.boosterIsAvailable = true;
        currentBooster.boosterIsActive = false;
        currentBooster.GetComponent<CanvasGroup>().blocksRaycasts = true;
        currentBooster.GetComponent<Button>().interactable = true;
    }

}


