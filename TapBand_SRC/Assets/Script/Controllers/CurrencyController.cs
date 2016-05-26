﻿using UnityEngine;
using System.Collections;

public class CurrencyController : MonoBehaviour
{

    private SongController songController;
    private ConcertController concertController;
    private TourController tourController;
    private MerchController merchController;
    private SkillUpgradeUI[] skillUpgradeUIs;

    private CurrencyState currencyState;

    public event CurrencyEvent OnCurrencyChanged;
    public event CurrencyEvent OnInitialized;

    void Awake()
    {
        songController = FindObjectOfType<SongController>();
        concertController = FindObjectOfType<ConcertController>();
        tourController = FindObjectOfType<TourController>();
        merchController = FindObjectOfType<MerchController>();
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

        merchController.MerchTransaction += MerchTransaction;
        merchController.CoinTransaction += AddCoins;
        
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

        merchController.MerchTransaction -= MerchTransaction;
        merchController.CoinTransaction -= AddCoins;
        
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

    private void MerchTransaction(MerchData merch)
    {
        currencyState.Coins -= merch.cost;

        SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        if (e.Status == SongStatus.Successful)
        {
            AddCoins(e.Data.coinReward);
        }
    }

    private void AddCoins(double coins)
    {
        currencyState.Coins += coins;
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
