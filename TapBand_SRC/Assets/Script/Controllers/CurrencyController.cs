﻿using UnityEngine;
using System.Collections;
using System;

public class CurrencyController : MonoBehaviour
{
    private SongController songController;
    private ConcertController concertController;
    private TourController tourController;
    private DailyEventController dailyEventController;
    private SkillUpgradeUI[] skillUpgradeUIs;

    private CurrencyState currencyState;

    public event CurrencyEvent OnCurrencyChanged;
    public event CurrencyEvent OnInitialized;

    void Awake()
    {
        songController = FindObjectOfType<SongController>();
        concertController = FindObjectOfType<ConcertController>();
        tourController = FindObjectOfType<TourController>();
        dailyEventController = FindObjectOfType<DailyEventController>();

        skillUpgradeUIs = FindObjectsOfType<SkillUpgradeUI>();

        currencyState = GameState.instance.Currency;
    }

    void Start()
    {
        if (OnInitialized != null)
        {
            OnInitialized(this, new CurrencyEventArgs(currencyState.Coins, currencyState.Fans, currencyState.Tokens));
        }
    }

    void OnEnable()
    {
        songController.OnSongFinished += HandleSongFinished;
        concertController.OnConcertFinished += HandleConcertFinished;
        tourController.OnPrestige += OnPrestige;

        dailyEventController.OnDailyEventFinished += HandleDailyEventFinished;

        for (int i = 0; i < skillUpgradeUIs.Length; i++)
        {
            skillUpgradeUIs[i].OnSkillUpgrade += HandleSkillUpgrade;
        }
    }

    void OnDisable()
    {
        songController.OnSongFinished -= HandleSongFinished;
        concertController.OnConcertFinished -= HandleConcertFinished;
        tourController.OnPrestige -= OnPrestige;

        dailyEventController.OnDailyEventFinished -= HandleDailyEventFinished;

        for (int i = 0; i < skillUpgradeUIs.Length; i++)
        {
            skillUpgradeUIs[i].OnSkillUpgrade -= HandleSkillUpgrade;
        }
    }

    public double TapMultiplierFromPrestige
    {
        get
        {
            return currencyState.TapMultiplierFromPrestige; 
        }
    }

    public bool CanBuyFromCoin(double price)
    {
        return currencyState.Coins >= price;
    }

    public bool CanBuyFromToken(int price)
    {
        return currencyState.Tokens >= price;
    }

    public void BuyFromCoin(double price)
    {
        if (!CanBuyFromCoin(price))
        {
            return;
        }
        currencyState.Coins -= price;
    }

    public void BuyFromToken(int price)
    {
        if (!CanBuyFromToken(price))
        {
            return;
        }
        // TODO: should request confirmation
        currencyState.Tokens -= price;
    }

    private void OnPrestige() 
    {
        //elveszik
        currencyState.Coins = 0;

        double tapStrengthMultiplier = 1.2f;                                 //TODO: képlettel meghatározni a pontos értékét egy fvben
        currencyState.TapMultiplierFromPrestige *= tapStrengthMultiplier;

        print("new tapStrength bonus after Prestige: "+ currencyState.TapMultiplierFromPrestige);
        

        SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void HandleSkillUpgrade(object sender, BandMemberSkillEventArgs e)
    {
        currencyState.Coins -= e.UnlockedSkill.upgradeCost;

        SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        if (e.Status == SongStatus.Successful)
        {
            AddCoins(e.Data.coinReward);
        }
    }

    public void AddCoins(double coins)
    {
        currencyState.Coins += Math.Floor(coins); // Currencies are using doubles
        SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void HandleConcertFinished(object sender, ConcertEventArgs e)
    {
        currencyState.Fans += e.Data.fanReward;
        SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void AddTokens(int tokens)
    {
        currencyState.Tokens += tokens;
        SynchronizeRealCurrencyAndScreenCurrency();
    }

    public void SynchronizeRealCurrencyAndScreenCurrency()
    {
        if (OnCurrencyChanged != null)
        {
            OnCurrencyChanged(this, new CurrencyEventArgs(currencyState.Coins, currencyState.Fans, currencyState.Tokens));
        }
    }

    private void HandleDailyEventFinished(object sender, DailyEventEventArgs e)
    {
        if (e.DailyStreakReward.tokenAmount > 0)
        {
            AddTokens(e.DailyStreakReward.tokenAmount);
        }
        else
        {
            var coinReward = concertController.CurrentConcertData.rewardBase * e.DailyStreakReward.coinMultiplier;
            AddCoins(coinReward);
        }

        if (e.DailyRandomReward.tokenAmount > 0)
        {
            AddTokens(e.DailyRandomReward.tokenAmount);
        }
        else if (e.DailyRandomReward.coinMultiplier > 0)
        {
            var coinReward = concertController.CurrentConcertData.rewardBase * e.DailyRandomReward.coinMultiplier;
            AddCoins(coinReward);
        }
    }


    //DEBUG CONTROLLER-----------------------------
    public void GiveCoins(double coins)
    {
        currencyState.Coins += coins;
        SynchronizeRealCurrencyAndScreenCurrency();
    }

    public void GiveFans(double fans)
    {
        currencyState.Fans += fans;
        SynchronizeRealCurrencyAndScreenCurrency();
    }

    public void GiveTokens(int tokens)
    {
        currencyState.Tokens += tokens;
        SynchronizeRealCurrencyAndScreenCurrency();
    }
}
