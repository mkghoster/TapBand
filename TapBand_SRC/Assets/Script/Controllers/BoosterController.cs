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
    private float helpme=0.30f;
    private int check=0;

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

            //Debug.Log(CurrentAutoTapBoosterDuration);

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

        if (CurrentAutoTapBoosterDuration > 0)
        {
            //		Debug.Log (CurrentAutoTapBoosterDuration);
        }
    }



    public void HandleBoosters(BoosterUI bd)
    {
        if (bd.name.Equals("TapStrengthBooster"))
        {
            tapController.BoosterMultiplier(boosterData.TapStrengthBoosterMultiplier);
            tapController.BoosterTimeInterval(boosterData.TapStrengthBoosterDuration);
            CurrentBoosterDuration = boosterData.TapStrengthBoosterDuration;
            tapStrengthBoosterisActive = true;
            StartCoroutine(SetBoosterIsActive(bd));
        }
        else if (bd.name.Equals("AutoTapBooster"))
        {

            CurrentAutoTapBoosterDuration = boosterData.AutoTapBoosterDuration;
           // Debug.Log(CurrentAutoTapBoosterDuration);
            autoTapisActive = true;
            CurrentBoosterDuration = CurrentAutoTapBoosterDuration;
            StartCoroutine(SetBoosterIsActive(bd));
            //for (int i = 0; i < boosterData.AutoTapBoosterDuration; i++) {            
            //    StartCoroutine(WaitForOneSecond());
            //}
        }
        else if (bd.name.Equals("ExtraTimeBooster"))
        {
            songController.BossExtratime(boosterData.ExtraTimeBoosterBonus);
            CurrentBoosterDuration = boosterData.ExtraTimeBoosterBonus;
            StartCoroutine(SetBoosterIsActive(bd));
        }
    }

    IEnumerator SetBoosterIsActive(BoosterUI bd)
    {
        bd.boosterIsActive = true;
        bd.boosterIsAvailable = false;
        bd.GetComponent<Button>().interactable = false;
        Debug.Log(bd.name+ " is unavailable for actions");
        yield return new WaitForSeconds(CurrentBoosterDuration);
        Debug.Log(bd.name+" is available again");
        bd.boosterIsAvailable = true;
        bd.boosterIsActive = false;
        bd.GetComponent<CanvasGroup>().blocksRaycasts = true;
        bd.GetComponent<Button>().interactable = true;
       
    }

}


