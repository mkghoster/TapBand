using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class DailyEventUI : MonoBehaviour
{
    public event DailyEventEvent OnDailyEventFinished;

    private GameObject dailyEventPanel;
    private DailyEventController dailyEventController;

    private DailyStreakData currentDailyStreakReward;
    private DailyRandomData currentDailyRandomReward;
    private int selectedRandomRewardIndex = -1;

    private Button[] randomEventButtons;
    private Text[] randomEventButtonTexts;

    //For debug purposes
    private Text dailyStreakDebugText;

    void Awake()
    {
        dailyEventPanel = transform.GetChild(0).gameObject;
        dailyEventController = FindObjectOfType<DailyEventController>();
        // for debug purposes
        dailyStreakDebugText = transform.GetChild(0).GetChild(0).GetComponent<Text>();

        randomEventButtons = new Button[3];
        randomEventButtons[0] = dailyEventPanel.transform.FindChild("RandomItem0Button").gameObject.GetComponent<Button>();
        randomEventButtons[1] = dailyEventPanel.transform.FindChild("RandomItem1Button").gameObject.GetComponent<Button>();
        randomEventButtons[2] = dailyEventPanel.transform.FindChild("RandomItem2Button").gameObject.GetComponent<Button>();

        randomEventButtonTexts = new Text[3];
        randomEventButtonTexts[0] = randomEventButtons[0].transform.GetChild(0).gameObject.GetComponent<Text>();
        randomEventButtonTexts[1] = randomEventButtons[1].transform.GetChild(0).gameObject.GetComponent<Text>();
        randomEventButtonTexts[2] = randomEventButtons[2].transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    void OnEnable()
    {
        dailyEventController.OnDailyEventStarted += HandleDailyEventStarted;
    }

    void OnDisable()
    {
        dailyEventController.OnDailyEventStarted -= HandleDailyEventStarted;
    }

    // for debug purposes
    public void OnDailyRandomButtonClick(int index)
    {
        if (selectedRandomRewardIndex == -1)
        {
            var items = dailyEventController.GetDailyRandomItems();
            selectedRandomRewardIndex = index;
            currentDailyRandomReward = items[index];
            SetButtonStates();
            ShowRewards(items);
        }
        else
        {
            FinishDailyRandomEvent();
        }
    }

    public void FinishDailyRandomEvent()
    {
        if (OnDailyEventFinished != null)
        {
            OnDailyEventFinished(this, new DailyEventEventArgs(currentDailyStreakReward, currentDailyRandomReward));
        }
        SetUiActive(false);
    }

    public void SetUiActive(bool active)
    {
        dailyEventPanel.SetActive(active);
    }

    private void HandleDailyEventStarted(object sender, DailyEventEventArgs e)
    {
        SetUiActive(true);
        currentDailyStreakReward = e.DailyStreakReward;

        dailyStreakDebugText.text = String.Format("Daily reward id: {0}, coins: {1}, tokens: {2}", currentDailyStreakReward.id, currentDailyStreakReward.coinMultiplier, currentDailyStreakReward.tokenAmount);
    }

    private void ResetUI()
    {
        currentDailyRandomReward = null;
        currentDailyStreakReward = null;
        selectedRandomRewardIndex = -1;
        SetButtonStates();
    }

    private void SetButtonStates()
    {
        if (selectedRandomRewardIndex >= 0)
        {
            randomEventButtons[0].interactable = (selectedRandomRewardIndex == 0);
            randomEventButtons[1].interactable = (selectedRandomRewardIndex == 1);
            randomEventButtons[2].interactable = (selectedRandomRewardIndex == 2);
        }
        else
        {
            randomEventButtons[0].interactable = true;
            randomEventButtons[1].interactable = true;
            randomEventButtons[2].interactable = true;
        }
    }

    private void ShowRewards(IList<DailyRandomData> items)
    {
        randomEventButtonTexts[0].text = items[0].name;
        randomEventButtonTexts[1].text = items[1].name;
        randomEventButtonTexts[2].text = items[2].name;

        randomEventButtonTexts[selectedRandomRewardIndex].fontStyle = FontStyle.Bold;
    }
}
