using UnityEngine;
using System.Collections;

public class CurrencyController : MonoBehaviour {

    private TourController tourController;
    private MerchController merchController;
    private EquipmentController equipmentController;

    private HudUI hudUI;

    void Awake()
    {
        tourController = (TourController)FindObjectOfType(typeof(TourController));
        merchController = (MerchController)FindObjectOfType(typeof(MerchController));
        equipmentController = (EquipmentController)FindObjectOfType(typeof(EquipmentController));
        hudUI = (HudUI)FindObjectOfType(typeof(HudUI));
    }

    void OnEnable()
    {
        tourController.ResetCurrencies += Reset;
        merchController.CoinTransaction += CoinTransaction;
        equipmentController.CoinTransaction += CoinTransaction;
        merchController.CanBuy += CanBuy;
        hudUI.NewCoin += Coins;
    }

    void OnDisable()
    {
        tourController.ResetCurrencies -= Reset;
        merchController.CoinTransaction -= CoinTransaction;
        equipmentController.CoinTransaction -= CoinTransaction;
        merchController.CanBuy -= CanBuy;
        hudUI.NewCoin -= Coins;
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

    private bool CanBuy(int price)
    {
        return GameState.instance.Currency.NumberOfCoins >= price;
    }

    private string Coins()
    {
        // Temporal solution for test purposes
        return GameState.instance.Currency.NumberOfCoins.ToString();
    }
}
