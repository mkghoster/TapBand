using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BoosterController : MonoBehaviour {

    enum BoosterDataType { TapStrengthBoosterMultiplier, TapStrengthBoosterDuration, ExtraTimeBoosterBonus, AutoTapBoosterInterval, AutoTapBoosterDuration };
    private float CurrentTapStrengthBoosterDuration;
    private float CurrentAutoTapBoosterDuration;
    private bool tapStrengthBoosterisActive = false;
    private bool autoTapisActive = false;
    private TapController tapController;
    private SongController songController;
    private BoosterData boosterData;

    public delegate void TapEvent(TapArgs args);
    public event TapEvent OnTap;

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
            CurrentTapStrengthBoosterDuration -= Time.deltaTime;
        }
        if (CurrentTapStrengthBoosterDuration <= 0)
        {
            tapStrengthBoosterisActive = false;
            tapController.BoosterTimeInterval(0);
        }

        if (autoTapisActive)
        {
            CurrentAutoTapBoosterDuration -= Time.deltaTime;
           // Debug.Log(CurrentAutoTapBoosterDuration);
        }

        if (CurrentAutoTapBoosterDuration <= 0)
        {
            autoTapisActive = false;
        }

        if (CurrentAutoTapBoosterDuration > 0)
        {
            //		Debug.Log (CurrentAutoTapBoosterDuration);
        }
    }



    public void HandleBoosters(string BoosterName, BoosterUI bd)
    {
        if (BoosterName.Equals("TapStrengthBooster"))
        {
            tapController.BoosterMultiplier(boosterData.TapStrengthBoosterMultiplier);
            tapController.BoosterTimeInterval(boosterData.TapStrengthBoosterDuration);
            CurrentTapStrengthBoosterDuration = boosterData.TapStrengthBoosterDuration;
            tapStrengthBoosterisActive = true;
            //Debug.Log(bd.IsActive());
            StartCoroutine(SetBoosterIsActive(bd));
        }
        else if (BoosterName.Equals("AutoTapBooster"))
        {
            CurrentAutoTapBoosterDuration = boosterData.AutoTapBoosterDuration;
            autoTapisActive = true;       
            for (int i = 0; i < boosterData.AutoTapBoosterDuration; i++) {            
                StartCoroutine(WaitForOneSecond());
            }
        }
        else if (BoosterName.Equals("ExtraTimeBooster"))
        {
            songController.BossExtratime(boosterData.ExtraTimeBoosterBonus);
        }
    }

    IEnumerator WaitForOneSecond()
    {
        int tapFrequency = 3;
        for (int i = 0; i < tapFrequency; i++)
        {
            HandleAutoTap();
        }
        yield return new WaitForSeconds(1);

    }

    public void HandleAutoTap()
    {
        {
          
        }
    }

    IEnumerator SetBoosterIsActive(BoosterUI bd)
    {
        Debug.Log(bd.gameObject.name);
        Debug.Log(bd.IsActive());
        bd.boosterIsActive = false;
        Debug.Log(bd.IsActive());
        yield return new WaitForSeconds(CurrentTapStrengthBoosterDuration);
        Debug.Log("most");
        bd.boosterIsActive = true;
        Debug.Log(bd.IsActive());
       
    }
}


