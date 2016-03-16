using UnityEngine;
using System.Collections;

public class CurrencyController : MonoBehaviour {

	private SongController songController;
	private ConcertController concertController;
    private TourController tourController;
    private MerchController merchController;
    private EquipmentController equipmentController;

    private HudUI hudUI;

    void Awake()
    {
		songController = (SongController)FindObjectOfType (typeof(SongController));
		concertController = (ConcertController)FindObjectOfType (typeof(ConcertController));
        tourController = (TourController)FindObjectOfType(typeof(TourController));
        merchController = (MerchController)FindObjectOfType(typeof(MerchController));
        equipmentController = (EquipmentController)FindObjectOfType(typeof(EquipmentController));
        hudUI = (HudUI)FindObjectOfType(typeof(HudUI));
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
        GameState.instance.Currency.Coins = 0;
        GameState.instance.Currency.Fans = 0;
        GameState.instance.Currency.AddTapMultiplier(tour.tapStrengthMultiplier);
    }

    private void EquipmentTransaction(EquipmentData equipment)
    {
        GameState.instance.Currency.Coins -= equipment.upgradeCost;
        GameState.instance.Currency.AddTapMultiplier(equipment.tapMultiplier);
    }

    private void MerchTransaction(MerchData merch)
    {
        GameState.instance.Currency.Coins -= merch.upgradeCost;
    }

    private void AddCoins(int coins)
    {
        GameState.instance.Currency.Coins += coins;
    }

    private void AddFans(int fans)
	{
        GameState.instance.Currency.Fans += fans;
	}

    private void AddTokens(int tokens)
    {
        GameState.instance.Currency.Tokens += tokens;
    }

    private bool CanBuy(int price)
    {
        return GameState.instance.Currency.Coins >= price;
    }

    private string Coins()
    {
        // Temporal solution for test purposes
        return GameState.instance.Currency.Coins.ToString();
    }

	private string Fans()
	{
		// Temporal solution for test purposes
		return GameState.instance.Currency.Fans.ToString();
	}
}
