using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoosterController : MonoBehaviour {

    enum BoosterDataType { TapStrengthBoosterMultiplier, TapStrengthBoosterDuration, ExtraTimeBoosterBonus, AutoTapBoosterInterval, AutoTapBoosterDuration };
    private float CurrentBoosterDuration;
    private float CurrentAutoTapBoosterDuration;
    private bool tapStrengthBoosterisActive = false;
    private bool autoTapisActive = false;
    private TapController tapController;
    private SongController songController;
    private BoosterData boosterData;
    //TODO AutoTapPerSec
    private float helpme=0.30f;
    //TODO AutoTapPerSec*AutoTapBoosterInterval
    private int check=0;

   // public delegate void TapEvent(TapArgs args);
   // public event TapEvent OnTap;

    private TapUI tapUI;

    void Start () {
        boosterData = GameData.instance.BoosterData;
        tapController = (TapController)FindObjectOfType(typeof(TapController));
        songController = (SongController)FindObjectOfType(typeof(SongController));
        tapUI = (TapUI)FindObjectOfType(typeof(TapUI));
    }
	
	void Update () {

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
            if (CurrentAutoTapBoosterDuration < boosterData.AutoTapBoosterDuration - helpme)
            {
                helpme += 0.30f;
                check++;
                //Debug.Log(check);
                tapUI.AutoTap();
            }
        }

        if (CurrentAutoTapBoosterDuration <= 0 || check==30)
        {
            autoTapisActive = false;
            check = 0;
            helpme = 0.30f;
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
        //Debug.Log(currentBooster.name+ " is unavailable for actions");
        yield return new WaitForSeconds(CurrentBoosterDuration);
        Debug.Log(currentBooster.name+" is available again");
        currentBooster.boosterIsAvailable = true;
        currentBooster.boosterIsActive = false;
        currentBooster.GetComponent<CanvasGroup>().blocksRaycasts = true;
        currentBooster.GetComponent<Button>().interactable = true;    
    }

}


