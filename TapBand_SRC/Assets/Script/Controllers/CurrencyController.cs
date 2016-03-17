using UnityEngine;
using System.Collections;

public class CurrencyController : MonoBehaviour {

	private SongController songController;
	private ConcertController concertController;
    private TourController tourController;
    private MerchController merchController;
    private EquipmentController equipmentController;

    private HudUI hudUI;

    private CurrencyState currencyState;

    void Awake()
    {
		songController = (SongController)FindObjectOfType (typeof(SongController));
		concertController = (ConcertController)FindObjectOfType (typeof(ConcertController));
        tourController = (TourController)FindObjectOfType(typeof(TourController));
        merchController = (MerchController)FindObjectOfType(typeof(MerchController));
        equipmentController = (EquipmentController)FindObjectOfType(typeof(EquipmentController));
        hudUI = (HudUI)FindObjectOfType(typeof(HudUI));

        currencyState = GameState.instance.Currency;
    }

    void OnEnable()
    {
		songController.GiveRewardOfSong += AddCoins;
		concertController.GiveCoinRewardOfConcert += AddCoins;
		concertController.GiveFanRewardOfConcert += AddFans;
        tourController.OnPrestige += OnPrestige;

        merchController.MerchTransaction += MerchTransaction;
        merchController.CoinTransaction += AddCoins;
        equipmentController.EquipmentTransaction += EquipmentTransaction;
        merchController.CanBuy += CanBuy;
        hudUI.NewCoin += Coins;
		hudUI.NewFans += Fans;
    }

    void OnDisable()
    {
		songController.GiveRewardOfSong -= AddCoins;
		concertController.GiveCoinRewardOfConcert -= AddCoins;
		concertController.GiveFanRewardOfConcert -= AddFans;
        tourController.OnPrestige -= OnPrestige;

        merchController.MerchTransaction -= MerchTransaction;
        merchController.CoinTransaction -= AddCoins;
        equipmentController.EquipmentTransaction -= EquipmentTransaction;
        merchController.CanBuy -= CanBuy;
        hudUI.NewCoin -= Coins;
		hudUI.NewFans -= Fans;
    }

    private void OnPrestige(TourData tour)
    {
        currencyState.Coins = 0;
        currencyState.Fans = 0;
        currencyState.AddTapMultiplier(tour.tapStrengthMultiplier);

        currencyState.SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void EquipmentTransaction(EquipmentData equipment)
    {
        currencyState.Coins -= equipment.upgradeCost;
        currencyState.AddTapMultiplier(equipment.tapMultiplier);

        currencyState.SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void MerchTransaction(MerchData merch)
    {
        currencyState.Coins -= merch.upgradeCost;

        currencyState.SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void AddCoins(int coins)
    {
        currencyState.Coins += coins;
        currencyState.SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void AddFans(int fans)
	{
        currencyState.Fans += fans;
        currencyState.SynchronizeRealCurrencyAndScreenCurrency();
    }

    private void AddTokens(int tokens)
    {
        currencyState.Tokens += tokens;
    }

    private bool CanBuy(int price)
    {
        return currencyState.Coins >= price;
    }

    private string Coins()
    {
        // Temporal solution for test purposes
        return currencyState.ScreenCoins.ToString();
    }

	private string Fans()
	{
		// Temporal solution for test purposes
		return currencyState.ScreenFans.ToString();
	}
}
