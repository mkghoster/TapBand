using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MerchSlotItemUI : MonoBehaviour
{
    public Button CollectActivateButton;
    public Text CollectActivateButtonText;
    public Image MerchImage;
    public Text TimeLeftText;
    public Text CollectedCoinsText;
    public Text SlotEmptyText;

    #region Private fields
    private MerchSlotState merchSlotState;
    private MerchSlotUI merchSlotUI;
    #endregion

    void Start()
    {
        CollectActivateButton.onClick.AddListener(() => OnCollectActivatePressed());
    }

    void Update()
    {
    }

    public void SetupItem(MerchSlotUI UI, MerchSlotState state)
    {
        merchSlotUI = UI;
        // MerchImage set here...
        UpdateItem(state);
    }

    public void UpdateItem(MerchSlotState state)
    {
        merchSlotState = state;
        UpdateItem();
    }

    public void UpdateItem()
    {
        switch (merchSlotState.Status)
        {
            case MerchSlotStatus.ACTIVE:
                SetComponentsToActive();
                CollectActivateButtonText.text = "Collect now" + Environment.NewLine + "(" +
                    merchSlotState.ActiveMerchState.TokenToFinish.ToString() + " token)";
                TimeSpan time = TimeSpan.FromSeconds(merchSlotState.ActiveMerchState.SecsToFinish);
                TimeLeftText.text = time.ToString();
                CollectedCoinsText.text = merchSlotState.ActiveMerchState.CollectedCoins.ToString("C2");
                break;

            case MerchSlotStatus.CLOSED:
                SetComponentsToClosed();
                if (merchSlotState.TokenCost == 0)
                {
                    CollectActivateButtonText.text = "Unlock" + Environment.NewLine + "(" + merchSlotState.CoinCost.ToString("C2") + ")";
                }
                else
                {
                    CollectActivateButtonText.text = "Unlock" + Environment.NewLine + "(" + merchSlotState.TokenCost.ToString() + " token)";
                }
                break;

            case MerchSlotStatus.COMPLETE:
                SetComponentsToClosed();
                CollectActivateButtonText.text = "Collect";
                CollectedCoinsText.text = merchSlotState.ActiveMerchState.CollectedCoins.ToString("C2");
                break;

            case MerchSlotStatus.EMPTY:
                SetComponentsToEmpty();
                break;
        }
    }

    private void SetComponentsToActive()
    {
        CollectActivateButton.gameObject.SetActive(true);
        MerchImage.gameObject.SetActive(true);
        TimeLeftText.gameObject.SetActive(true);
        CollectedCoinsText.gameObject.SetActive(true);
        SlotEmptyText.gameObject.SetActive(false);
    }

    private void SetComponentsToClosed()
    {
        CollectActivateButton.gameObject.SetActive(true);
        MerchImage.gameObject.SetActive(false);
        TimeLeftText.gameObject.SetActive(false);
        CollectedCoinsText.gameObject.SetActive(false);
        SlotEmptyText.gameObject.SetActive(false);
    }

    private void SetComponentsToEmpty()
    {
        CollectActivateButton.gameObject.SetActive(false);
        MerchImage.gameObject.SetActive(false);
        TimeLeftText.gameObject.SetActive(false);
        CollectedCoinsText.gameObject.SetActive(false);
        SlotEmptyText.gameObject.SetActive(true);
    }

    private void SetComponentsToComplete()
    {
        CollectActivateButton.gameObject.SetActive(true);
        MerchImage.gameObject.SetActive(true);
        TimeLeftText.gameObject.SetActive(false);
        CollectedCoinsText.gameObject.SetActive(true);
        SlotEmptyText.gameObject.SetActive(false);
    }

    private void OnCollectActivatePressed()
    {
        switch (merchSlotState.Status)
        {
            case MerchSlotStatus.ACTIVE:
                merchSlotUI.OnCollectPressed(merchSlotState.ActiveMerchState);
                break;

            case MerchSlotStatus.CLOSED:
                merchSlotUI.OnActivatePressed(merchSlotState);
                break;

            case MerchSlotStatus.COMPLETE:
                merchSlotUI.OnCollectPressed(merchSlotState.ActiveMerchState);
                break;
        }
    }
}
