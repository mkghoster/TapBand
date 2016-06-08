using System;
using System.Collections;
using UnityEngine;

public class BackstageController : MonoBehaviour
{
    public event Action OnMerchButtonPressed;
    public event Action OnPrestigeButtonPressed;
    public event Action OnDebugButtonPressed;
    public event Action OnSettingsButtonPressed;

    #region Private fields
    private BackstageUI backstageUI;
    private DressingRoomUI dressingRoomUI;
    private ViewController viewController;

    private BandMemberController bandMemberController;
    private CurrencyController currencyController;
    #endregion

    void Awake()
    {
        backstageUI = FindObjectOfType<BackstageUI>();

        dressingRoomUI = FindObjectOfType<DressingRoomUI>();

        viewController = FindObjectOfType<ViewController>();

        bandMemberController = FindObjectOfType<BandMemberController>();
        currencyController = FindObjectOfType<CurrencyController>();
    }

    void Start()
    {
        backstageUI.SetController(this);
        dressingRoomUI.SetController(this);
    }

    void OnEnable()
    {
        viewController.OnViewChange += ViewChanged;
        currencyController.OnCurrencyChanged += HandleCurrencyEvent;
    }

    void OnDisable()
    {
        viewController.OnViewChange -= ViewChanged;
        currencyController.OnCurrencyChanged += HandleCurrencyEvent;
    }

    private void ViewChanged(object sender, ViewChangeEventArgs e)
    {
        if (e.NewView == ViewType.BACKSTAGE)
        {
            backstageUI.ShowUI();
        }
    }

    public void SwitchToStage()
    {
        viewController.EnterView(ViewType.STAGE);
    }

    public void OnMerchClick()
    {
        if (OnMerchButtonPressed != null)
        {
            OnMerchButtonPressed();
        }
    }

    public void OnPrestigeClick()
    {
        if (OnPrestigeButtonPressed != null)
        {
            OnPrestigeButtonPressed();
        }
    }

    public void SwitchDressingRoom(bool dressingRoom)
    {
        if (dressingRoom)
        {
            viewController.EnterView(ViewType.CUSTOMIZATION); // TODO: animate camera
            dressingRoomUI.ShowUI();
        }
        else
        {
            viewController.EnterView(ViewType.BACKSTAGE);
            backstageUI.ShowUI();
        }
    }

    public void OnSettingsClick()
    {
        if(OnSettingsButtonPressed != null)
        {
            OnSettingsButtonPressed();
        }
    }

    public void OnDebugClick()
    {
        if (OnDebugButtonPressed != null)
        {
            OnDebugButtonPressed();
        }
    }

    public CharacterData GetNextUpgrade(CharacterType character)
    {
        return bandMemberController.NextUpgrades[character];
    }

    public bool CanBuyNextUpgrade(CharacterType bandMember)
    {
        return bandMemberController.CanBuyNextUpgrade(bandMember);
    }

    public void UpdateCharacter(CharacterType bandMember)
    {
        if (CanBuyNextUpgrade(bandMember))
        {
            bandMemberController.UpgradeSkill(bandMember);
        }
    }

    private void HandleCurrencyEvent(object sender, CurrencyEventArgs e)
    {
        dressingRoomUI.UpdateUI();
    }
}
