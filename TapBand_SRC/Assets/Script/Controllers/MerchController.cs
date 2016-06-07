using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class MerchController : MonoBehaviour
{
    #region Private fields
    private MerchUI merchUI;
    private MerchSlotUI merchSlotUI;
    private CurrencyController currencyController;
    private ViewController viewController;
    private BackstageController backstageController;
    #endregion

    void Awake()
    {
        currencyController = FindObjectOfType<CurrencyController>();
        viewController = FindObjectOfType<ViewController>();
        backstageController = FindObjectOfType<BackstageController>();

        merchUI = FindObjectOfType<MerchUI>();
        merchUI.SetController(this);
        merchSlotUI = FindObjectOfType<MerchSlotUI>();
        merchSlotUI.SetController(this);

        backstageController.OnMerchButtonPressed += merchUI.ShowUI;
    }

    void Start()
    {
        merchUI.CreateMerchItems(GameState.instance.MerchStates);
        merchSlotUI.CreateMerchSlotItems(GameState.instance.MerchSlotStates);
    }

    void Update()
    {
        merchUI.UpdateMerchItems();
        merchSlotUI.UpdateMerchSlotItems();
    }

    public bool HasFreeSlot()
    {
        return GetFirstFreeSlot() != null;
    }

    public MerchSlotState GetFirstFreeSlot()
    {
        for (int i = 0; i < GameState.instance.MerchSlotStates.Count; i++)
        {
            if (GameState.instance.MerchSlotStates[i].Status == MerchSlotStatus.EMPTY)
            {
                return GameState.instance.MerchSlotStates[i];
            }
        }
        return null;
    }

    public MerchSlotState GetSlotOfMerch(MerchType type)
    {
        return GameState.instance.MerchSlotStates.FirstOrDefault(c => c.ActiveMerchType == type);
    }

    public void OnStart(MerchState state)
    {
        MerchSlotState freeSlot = GetFirstFreeSlot();
        if (freeSlot == null)
        {
            return;
        }
        state.Start();
        if (state.Started)
        {
            freeSlot.ActiveMerchType = state.Type;
            merchUI.UpdateMerchItems();
            merchSlotUI.UpdateMerchSlotItems();
        }
    }

    public void OnCollect(MerchState state)
    {
        if (!state.CanCollect())
        {
            return;
        }
        currencyController.BuyFromToken(state.TokenToFinish);
        currencyController.AddCoins(state.CollectibleCoins);
        state.ResetTimer();
        MerchSlotState slotState = GetSlotOfMerch(state.Type);
        slotState.ActiveMerchType = MerchType.NONE;
        merchUI.UpdateMerchItems();
        merchSlotUI.UpdateMerchSlotItems();
    }

    public void OnUpgrade(MerchState state)
    {
        if (!state.CanUpgrade())
        {
            return;
        }
        state.Upgrade();
        currencyController.BuyFromCoin(state.UpgradeCost);
        merchUI.UpdateMerchItems();
        merchSlotUI.UpdateMerchSlotItems();
    }

    public void OnActivateSlot(MerchSlotState state)
    {
        if (!state.CanActivate())
        {
            return;
        }
        state.Activate();

        if (state.CoinCost > 0)
        {
            currencyController.BuyFromCoin(state.CoinCost);
        }
        else
        {
            currencyController.BuyFromToken(state.TokenCost);
        }

        merchSlotUI.UpdateMerchSlotItems();
    }

    public void OnClose()
    {
        viewController.EnterView(ViewType.BACKSTAGE);
    }
}
