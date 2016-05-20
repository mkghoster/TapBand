using UnityEngine;
using System.Collections;

public class CurrencyController : MonoBehaviour
{

    private SongController songController;
    private ConcertController concertController;
    private TourController tourController;
    private MerchController merchController;
    //private EquipmentController equipmentController;

    private HudUI hudUI;

    private CurrencyState currencyState;

    public event CurrencyEvent OnCurrencyChanged;
    public event CurrencyEvent OnInitialized;

    void Awake()
    {
        songController = (SongController)FindObjectOfType(typeof(SongController));
        concertController = (ConcertController)FindObjectOfType(typeof(ConcertController));
        tourController = (TourController)FindObjectOfType(typeof(TourController));
        merchController = (MerchController)FindObjectOfType(typeof(MerchController));
        //equipmentController = (EquipmentController)FindObjectOfType(typeof(EquipmentController));
        hudUI = (HudUI)FindObjectOfType(typeof(HudUI));

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
        concertController.OnConcertFinished += AddFans;
        tourController.OnPrestige += OnPrestige;

        merchController.MerchTransaction += MerchTransaction;
        merchController.CoinTransaction += AddCoins;
        //equipmentController.EquipmentTransaction += EquipmentTransaction;
        merchController.CanBuy += CanBuy;
    }

    void OnDisable()
    {
        songController.OnSongFinished -= HandleSongFinished;
        concertController.OnConcertFinished -= AddFans;
        tourController.OnPrestige -= OnPrestige;

        merchController.MerchTransaction -= MerchTransaction;
        merchController.CoinTransaction -= AddCoins;
        //equipmentController.EquipmentTransaction -= EquipmentTransaction;
        merchController.CanBuy -= CanBuy;
    }

    private void OnPrestige()
    {
        currencyState.Coins = 0;
        currencyState.Fans = 0;
        //  currencyState.AddTapMultiplier(tour.tapStrengthMultiplier);

        SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void EquipmentTransaction(CharacterData equipment)
    {
        currencyState.Coins -= equipment.upgradeCost;
        currencyState.TapMultipliers.Add(equipment.tapStrengthBonus);

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

    private void AddFans(object sender, ConcertEventArgs e)
    {
        currencyState.Fans += e.Data.fanReward;
        SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void AddTokens(int tokens)
    {
        currencyState.Tokens += tokens;
    }

    private bool CanBuy(int price)
    {
        return currencyState.Coins >= price;
    }

    public void SynchronizeRealCurrencyAndScreenCurrency()
    {
        if (OnCurrencyChanged != null)
        {
            OnCurrencyChanged(this, new CurrencyEventArgs(currencyState.Coins, currencyState.Fans, currencyState.Tokens)); //TODO: ez nem teljesen korrekt így
        }
    }
}
