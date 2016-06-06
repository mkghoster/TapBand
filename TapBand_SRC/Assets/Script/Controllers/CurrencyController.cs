using UnityEngine;
using System.Collections;
using System;

public class CurrencyController : MonoBehaviour
{
    private SongController songController;
    private ConcertController concertController;
    private TourController tourController;
    private BoosterController boosterController;
    private DailyEventController dailyEventController;
    private SkillUpgradeUI[] skillUpgradeUIs;
    private IapData iapData;

    private CurrencyState currencyState;
    private DailyEventState dailyEventState;

    public event CurrencyEvent OnCurrencyChanged;
    public event CurrencyEvent OnInitialized;

    void Awake()
    {
        songController = FindObjectOfType<SongController>();
        concertController = FindObjectOfType<ConcertController>();
        tourController = FindObjectOfType<TourController>();
        dailyEventController = FindObjectOfType<DailyEventController>();
        boosterController = FindObjectOfType<BoosterController>();

        skillUpgradeUIs = FindObjectsOfType<SkillUpgradeUI>();

        currencyState = GameState.instance.Currency;
        dailyEventState = GameState.instance.DailyEvent;

        iapData = GameData.instance.IapData;
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
        boosterController.OnBoosterActivated += HandleBoosterActivated;



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

//        dailyEventController.OnDailyEventFinished -= HandleDailyEventFinished;

        boosterController.OnBoosterActivated -= HandleBoosterActivated;



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
        currencyState.FanBonusPerTour.Add(currencyState.FanFromActualTour);
        currencyState.FanFromActualTour = 0;

        double tapStrengthMultiplier = CalculateTapStrengthBonus();                                 
        currencyState.TapMultiplierFromPrestige *= tapStrengthMultiplier;

        print("new tapStrength bonus after Prestige: "+ currencyState.TapMultiplierFromPrestige);
        
        //for(int i = 0; i < currencyState.FanBonusPerTour.Count; i++) { print(i + ".: elem: " + currencyState.FanBonusPerTour[i]); }

        SynchronizeRealCurrencyAndScreenCurrency();
    }

    //TODO: implement it
    private double CalculateTapStrengthBonus()
    {
        return 1.2;
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
        currencyState.FanFromActualTour += e.Data.fanReward;
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

    private void HandleBoosterActivated(object sender, BoosterEventArgs e)
    {
        switch (e.Type)
        {
            case BoosterType.AutoTap:
                currencyState.Tokens -= Mathf.FloorToInt(((float)iapData.autoTapBoosterCost) * dailyEventState.AutoTapBoosterPriceMultiplier);
                break;
            case BoosterType.ExtraTime:
                currencyState.Tokens -= Mathf.FloorToInt(((float)iapData.extraTimeBoosterCost) * dailyEventState.ExtraTimeBoosterPriceMultiplier);
                break;
            case BoosterType.TapStrength:
                currencyState.Tokens -= Mathf.FloorToInt(((float)iapData.tapStrenghtBoosterCost) * dailyEventState.TapStrengthBoosterPriceMultiplier);
                break;
            default:
                throw new NotImplementedException("This booster cost is not implemented");

        }
        SynchronizeRealCurrencyAndScreenCurrency();
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
