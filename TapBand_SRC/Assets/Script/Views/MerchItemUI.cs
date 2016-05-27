using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MerchItemUI : MonoBehaviour
{
    public Text NameText;
    public Image MerchImage;
    public Text TimeLeftText;
    public Text CollectedCoinsText;
    public Text LevelText;
    public Button StartCollectButton;
    public Button UpgradeButton;
    public Text StartCollectButtonText;
    public Text UpgradeButtonText;

    #region Private fields
    //private int merchId;
    private MerchState merchState;
    private MerchUI merchUI;
    #endregion

    void Start()
    {
        UpgradeButton.onClick.AddListener(() => OnUpgradePressed());
        StartCollectButton.onClick.AddListener(() => OnStartCollectPressed());
    }

    void Update()
    {
    }

    public void SetupItem(MerchUI UI, MerchState state)
    {
        merchUI = UI;
        // MerchImage set here...
        UpdateItem(state);
    }

    public void UpdateItem(MerchState state)
    {
        merchState = state;
        UpdateItem();
    }

    public void UpdateItem()
    {
        if (merchState.Activated)
        {
            UpgradeButtonText.text = "Upgrade" + Environment.NewLine + "(" + merchState.UpgradeCost.ToString("C2") + ")";
        }
        else
        {
            UpgradeButtonText.text = "Activate" + Environment.NewLine + "(" + merchState.UpgradeCost.ToString("C2") + ")";
        }
        NameText.text = merchState.Name;
        TimeSpan time = TimeSpan.FromSeconds(merchState.SecsToFinish);
        TimeLeftText.text = time.ToString();
        if (merchState.Started)
        {
            if (merchState.SecsToFinish != 0)
            {
                StartCollectButtonText.text = "Collect now" + Environment.NewLine + "(" + merchState.TokenToFinish.ToString() + " token)";
            }
            else
            {
                StartCollectButtonText.text = "Collect";
            }
        }
        else
        {
            StartCollectButtonText.text = "Start";
            StartCollectButton.interactable = merchUI.HasFreeSlot();
        }
        StartCollectButton.gameObject.SetActive(merchState.MerchId != 0);
        LevelText.text = merchState.MerchId.ToString();
        CollectedCoinsText.text = merchState.CollectedCoins.ToString("C2");
    }

    private void OnStartCollectPressed()
    {
        if (merchState.Started)
        {
            merchUI.OnCollectPressed(merchState);
        }
        else
        {
            merchUI.OnStartPressed(merchState);
        }
    }

    private void OnUpgradePressed()
    {
        merchUI.OnUpgradePressed(merchState);
    }
}
