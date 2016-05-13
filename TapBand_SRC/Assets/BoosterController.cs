using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BoosterController : MonoBehaviour {

    enum BoosterDataType { TapStrengthBoosterMultiplier, TapStrengthBoosterDuration, ExtraTimeBoosterBonus, AutoTapBoosterInterval, AutoTapBoosterDuration };
    private float CurrentTapStrengthBoosterDuration;
    private float CurrentAutoTapBoosterDuration;
    private bool startCountDown = false;
    private bool autoTapStartCountDown = false;
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
           // Debug.Log(CurrentAutoTapBoosterDuration);
            

        }

        if (CurrentAutoTapBoosterDuration <= 0)
        {
            autoTapStartCountDown = false;
        }

        if (CurrentAutoTapBoosterDuration > 0)
        {
            //		Debug.Log (CurrentAutoTapBoosterDuration);
        }
    }



    public void HandleBoosters(string BoosterName)
    {
        if (BoosterName.Equals("TapStrengthBooster"))
        {
            tapController.BoosterMultiplier(GameData.instance.BoosterData.TapStrengthBoosterMultiplier);
            tapController.BoosterTimeInterval(GameData.instance.BoosterData.TapStrengthBoosterDuration);
            CurrentTapStrengthBoosterDuration = GameData.instance.BoosterData.TapStrengthBoosterDuration;
            startCountDown = true;
        }
        else if (BoosterName.Equals("AutoTapBooster"))
        {
            CurrentAutoTapBoosterDuration = GameData.instance.BoosterData.AutoTapBoosterDuration;
            autoTapStartCountDown = true;
           
            for (int i = 0; i < GameData.instance.BoosterData.AutoTapBoosterDuration; i++) { 
                
                StartCoroutine(WaitForOneSecond());
            }

        }
        else if (BoosterName.Equals("ExtraTimeBooster"))
        {
            songController.BossExtratime(GameData.instance.BoosterData.ExtraTimeBoosterBonus);
        }
    }

    IEnumerator WaitForOneSecond()
    {
        int tapFrequency = 3;
        for (int i = 0; i < tapFrequency-1; i++)
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
}


