using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BoosterController : MonoBehaviour {

    enum BoosterDataType { TapStrengthBoosterMultiplier, TapStrengthBoosterDuration, ExtraTimeBoosterBonus, AutoTapBoosterInterval, AutoTapBoosterDuration };
    private float TapStrengthBoosterMultiplier;
    private float TapStrengthBoosterDuration;
    private float CurrentTapStrengthBoosterDuration;
    private float ExtraTimeBoosterBonus;
    private float AutoTapBoosterInterval;
    private float AutoTapBoosterDuration;
    private float CurrentAutoTapBoosterDuration;
    private bool startCountDown = false;
    private bool autoTapStartCountDown = false;
    private TapController tapController;
    private SongController songController;
    private BoosterData boosterData;

    public delegate void TapEvent(float value);
    public event TapEvent OnTap;
    private TapUI tapUI;

    void Start () {
        boosterData.LoadBoostersData();
        tapController = (TapController)FindObjectOfType(typeof(TapController));
        songController = (SongController)FindObjectOfType(typeof(SongController));
        tapUI = (TapUI)FindObjectOfType(typeof(TapUI));
    }
	
	void Update () {

        if (startCountDown)
        {
            CurrentTapStrengthBoosterDuration -= Time.deltaTime;
        }
        if (CurrentTapStrengthBoosterDuration <= 0)
        {
            startCountDown = false;
            tapController.BoosterTimeInterval(0);
        }

        if (autoTapStartCountDown)
        {
            CurrentAutoTapBoosterDuration -= Time.deltaTime;
        }

        if (CurrentAutoTapBoosterDuration <= 0)
        {
            autoTapStartCountDown = false;
        }

        //		if (autoTapStartCountDown) {
        //			for (int i = 0; i < 10; i++) {
        //				if (Time.time == i) {
        //					
        //				}
        //			}		
        //		}
        if (CurrentAutoTapBoosterDuration > 0)
        {
            //		Debug.Log (CurrentAutoTapBoosterDuration);
        }
    }



    public void HandleBoosters(string BoosterName)
    {
        if (BoosterName.Equals("TapStrengthBooster"))
        {
            tapController.BoosterMultiplier(TapStrengthBoosterMultiplier);
            tapController.BoosterTimeInterval(TapStrengthBoosterDuration);
            CurrentTapStrengthBoosterDuration = TapStrengthBoosterDuration;
            startCountDown = true;
        }
        else if (BoosterName.Equals("AutoTapBooster"))
        {
            CurrentAutoTapBoosterDuration = AutoTapBoosterDuration;
            autoTapStartCountDown = true;

        }
        else if (BoosterName.Equals("ExtraTimeBooster"))
        {
            songController.BossExtratime(ExtraTimeBoosterBonus);
        }
    }
}
