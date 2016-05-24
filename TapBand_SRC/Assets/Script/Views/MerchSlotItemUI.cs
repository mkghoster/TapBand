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
    //public Text SlotCostText;

    #region Private fields
    private MerchSlotState merchSlotState;
    private MerchUI merchUI;
    #endregion

    void Start()
    {
        CollectActivateButton.onClick.AddListener(() => OnCollectActivatePressed());
    }

    void Update()
    {
    }

    public void SetupItem(MerchUI UI, MerchSlotState state)
    {
        merchUI = UI;
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
        CollectActivateButton.GetComponent<CanvasGroup>().alpha = 1;
        CollectActivateButton.GetComponent<CanvasGroup>().interactable = true;
        MerchImage.GetComponent<CanvasGroup>().alpha = 1;
        MerchImage.GetComponent<CanvasGroup>().interactable = true;
        TimeLeftText.GetComponent<CanvasGroup>().alpha = 1;
        TimeLeftText.GetComponent<CanvasGroup>().interactable = true;
        CollectedCoinsText.GetComponent<CanvasGroup>().alpha = 1;
        CollectedCoinsText.GetComponent<CanvasGroup>().interactable = true;
        SlotEmptyText.GetComponent<CanvasGroup>().alpha = 0;
        SlotEmptyText.GetComponent<CanvasGroup>().interactable = false;
        //SlotCostText.GetComponent<CanvasGroup>().alpha = 0;
        //SlotCostText.GetComponent<CanvasGroup>().interactable = false;
    }

    private void SetComponentsToClosed()
    {
        CollectActivateButton.GetComponent<CanvasGroup>().alpha = 1;
        CollectActivateButton.GetComponent<CanvasGroup>().interactable = true;
        MerchImage.GetComponent<CanvasGroup>().alpha = 0;
        MerchImage.GetComponent<CanvasGroup>().interactable = false;
        TimeLeftText.GetComponent<CanvasGroup>().alpha = 0;
        TimeLeftText.GetComponent<CanvasGroup>().interactable = false;
        CollectedCoinsText.GetComponent<CanvasGroup>().alpha = 0;
        CollectedCoinsText.GetComponent<CanvasGroup>().interactable = false;
        SlotEmptyText.GetComponent<CanvasGroup>().alpha = 0;
        SlotEmptyText.GetComponent<CanvasGroup>().interactable = false;
        //SlotCostText.GetComponent<CanvasGroup>().alpha = 1;
        //SlotCostText.GetComponent<CanvasGroup>().interactable = true;
    }

    private void SetComponentsToEmpty()
    {
        CollectActivateButton.GetComponent<CanvasGroup>().alpha = 0;
        CollectActivateButton.GetComponent<CanvasGroup>().interactable = false;
        MerchImage.GetComponent<CanvasGroup>().alpha = 0;
        MerchImage.GetComponent<CanvasGroup>().interactable = false;
        TimeLeftText.GetComponent<CanvasGroup>().alpha = 0;
        TimeLeftText.GetComponent<CanvasGroup>().interactable = false;
        CollectedCoinsText.GetComponent<CanvasGroup>().alpha = 0;
        CollectedCoinsText.GetComponent<CanvasGroup>().interactable = false;
        SlotEmptyText.GetComponent<CanvasGroup>().alpha = 1;
        SlotEmptyText.GetComponent<CanvasGroup>().interactable = true;
        //SlotCostText.GetComponent<CanvasGroup>().alpha = 0;
        //SlotCostText.GetComponent<CanvasGroup>().interactable = false;
    }

    private void SetComponentsToComplete()
    {
        CollectActivateButton.GetComponent<CanvasGroup>().alpha = 1;
        CollectActivateButton.GetComponent<CanvasGroup>().interactable = true;
        MerchImage.GetComponent<CanvasGroup>().alpha = 1;
        MerchImage.GetComponent<CanvasGroup>().interactable = true;
        TimeLeftText.GetComponent<CanvasGroup>().alpha = 0;
        TimeLeftText.GetComponent<CanvasGroup>().interactable = false;
        CollectedCoinsText.GetComponent<CanvasGroup>().alpha = 1;
        CollectedCoinsText.GetComponent<CanvasGroup>().interactable = true;
        SlotEmptyText.GetComponent<CanvasGroup>().alpha = 0;
        SlotEmptyText.GetComponent<CanvasGroup>().interactable = false;
        //SlotCostText.GetComponent<CanvasGroup>().alpha = 0;
        //SlotCostText.GetComponent<CanvasGroup>().interactable = false;
    }

    private void OnCollectActivatePressed()
    {
        switch (merchSlotState.Status)
        {
            case MerchSlotStatus.ACTIVE:
                merchUI.OnCollectPressed(merchSlotState.ActiveMerchState);
                break;

            case MerchSlotStatus.CLOSED:
                merchUI.OnActivatePressed(merchSlotState);
                break;

            case MerchSlotStatus.COMPLETE:
                merchUI.OnCollectPressed(merchSlotState.ActiveMerchState);
                break;
        }
    }
}
