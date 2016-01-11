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
		songController.GiveRewardOfSong += CoinTransaction;
		concertController.GiveCoinRewardOfConcert += CoinTransaction;
		concertController.GiveFanRewardOfConcert += FanTransaction;
        tourController.ResetCurrencies += Reset;
        merchController.CoinTransaction += CoinTransaction;
        equipmentController.CoinTransaction += CoinTransaction;
        merchController.CanBuy += CanBuy;
        hudUI.NewCoin += Coins;
		hudUI.NewFans += Fans;
    }

    void OnDisable()
    {
		songController.GiveRewardOfSong -= CoinTransaction;
		concertController.GiveCoinRewardOfConcert -= CoinTransaction;
		concertController.GiveFanRewardOfConcert -= FanTransaction;
        tourController.ResetCurrencies -= Reset;
        merchController.CoinTransaction -= CoinTransaction;
        equipmentController.CoinTransaction -= CoinTransaction;
        merchController.CanBuy -= CanBuy;
        hudUI.NewCoin -= Coins;
		hudUI.NewFans -= Fans;
    }

    private void Reset()
    {
        GameState.instance.Currency.NumberOfCoins = 0;
        GameState.instance.Currency.NumberOfFans = 0;
    }

    private void CoinTransaction(int price)
    {
        // TODO implement checks
        GameState.instance.Currency.NumberOfCoins += price;
    }

	private void FanTransaction(int fans)
	{
		GameState.instance.Currency.NumberOfFans += fans;
	}

    private bool CanBuy(int price)
    {
        return GameState.instance.Currency.NumberOfCoins >= price;
    }

    private string Coins()
    {
        // Temporal solution for test purposes
        return GameState.instance.Currency.NumberOfCoins.ToString();
    }

	private string Fans()
	{
		// Temporal solution for test purposes
		return GameState.instance.Currency.NumberOfFans.ToString();
	}
}
