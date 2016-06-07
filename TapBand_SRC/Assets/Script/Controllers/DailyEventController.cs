using UnityEngine;
using System;
using System.Collections.Generic;

public class DailyEventController : MonoBehaviour
{
    private DateTime lastTriggeredEvent;
    private DateTime resetTime;
    private DailyEventState state;

    private int resetHour;

    private DailyStreakData nextReward;
    private IList<DailyRandomData> dailyRandomData;
    private float dailyRandomTotalPossibility;


    private bool shouldTrigger;
    private bool shouldCheck;

    private DailyEventUI dailyEventUI;

    private ViewController viewController;

    public event DailyEventEvent OnDailyEventStarted;
    public event DailyEventEvent OnDailyEventFinished;

    void Awake()
    {
        var gameData = GameData.instance;

        viewController = FindObjectOfType<ViewController>();

        state = GameState.instance.DailyEvent;
        resetHour = gameData.GeneralData.DailyRandomResetHour;

        for (int i = 0; i < gameData.DailyStreakDataList.Count; i++)
        {
            var currentData = gameData.DailyStreakDataList[i];
            if (currentData.id == state.lastStreakReward + 1)
            {
                nextReward = currentData;
                break;
            }
        }

        dailyRandomData = gameData.DailyRandomDataList;
        dailyRandomTotalPossibility = 0;
        for (int i = 0; i < dailyRandomData.Count; i++)
        {
            dailyRandomTotalPossibility += dailyRandomData[i].possibility;
        }

        dailyEventUI = FindObjectOfType<DailyEventUI>();

        CalculateResetTime();

        shouldCheck = true;
    }

    void OnEnable()
    {
        // for debug purposes
        dailyEventUI.OnDailyEventFinished += HandleDailyEventCompleted;
    }

    void OnDisable()
    {
        // for debug purposes
        dailyEventUI.OnDailyEventFinished -= HandleDailyEventCompleted;
    }

    void Update()
    {
        if (shouldCheck)
        {
            shouldTrigger = resetTime < DateTime.Now;
        }
        if (shouldTrigger && viewController.CurrentView == ViewType.STAGE)
        {
            shouldTrigger = false;
            shouldCheck = false;
            TriggerDailyEvent();
        }
    }

    private void HandleDailyEventCompleted(object sender, DailyEventEventArgs e)
    {
        state.lastEventCompleted = DateTime.Now;
        CalculateResetTime();
        shouldCheck = true;

        if (e.DailyRandomReward.autoTapBoosterDiscount > 0)
        {
            state.autoTapBoosterDiscount = e.DailyRandomReward.autoTapBoosterDiscount;
            state.autoTapBoosterDiscountUntil = resetTime; // TODO: read time from param
        }
        else if (e.DailyRandomReward.extraTimeBoosterDiscount > 0)
        {
            state.extraTimeBoosterDiscount = e.DailyRandomReward.extraTimeBoosterDiscount;
            state.extraTimeBoosterDiscountUntil = resetTime; // TODO: read time from param
        }
        else if (e.DailyRandomReward.tapStrengthBoosterDiscount > 0)
        {
            state.tapStrengthBoosterDiscount = e.DailyRandomReward.tapStrengthBoosterDiscount;
            state.tapStrengthBoosterDiscountUntil = resetTime; // TODO: read time from param
        }


        if (OnDailyEventFinished != null)
        {
            OnDailyEventFinished(this, e);
        }

        viewController.EnterView(ViewType.STAGE);
    }

    private void CalculateResetTime()
    {
        lastTriggeredEvent = new DateTime(state.lastEventCompleted.Year, state.lastEventCompleted.Month, state.lastEventCompleted.Hour < resetHour ? state.lastEventCompleted.Day - 1 : state.lastEventCompleted.Day, resetHour, 0, 0);

        //calculate reset time
        resetTime = lastTriggeredEvent.AddDays(1);
        Debug.Log("DailyEventController: CalculateResetTime: " + resetTime.ToShortDateString());
    }

    private void TriggerDailyEvent()
    {
        viewController.EnterView(ViewType.DAILY_EVENT);
        if (OnDailyEventStarted != null)
        {
            OnDailyEventStarted(this, new DailyEventEventArgs(nextReward, null));
        }
    }

    public IList<DailyRandomData> GetDailyRandomItems()
    {
        var result = new DailyRandomData[3];
        for (int i = 0; i < 3; i++)
        {
            result[i] = GetDailyRandomItem();
        }
        return result;
    }

    private DailyRandomData GetDailyRandomItem()
    {
        var dailyRandomSelector = UnityEngine.Random.Range(0, dailyRandomTotalPossibility);
        float passedPossibility = 0;

        for (int i = 0; i < dailyRandomData.Count; i++)
        {

            var dataItem = dailyRandomData[i];
            passedPossibility += dataItem.possibility;
            if (passedPossibility >= dailyRandomSelector)
            {
                return dataItem;
            }
        }
        return null; // And make it double
    }
}
