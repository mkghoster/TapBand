using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SkillUpgradeUI : MonoBehaviour
{
    public CharacterType bandMember;
    public event BandMemberSkillEvent OnSkillUpgrade;

    private BandMemberController bandMemberController;

    private Text childText; //TODO: implement final ui 
    private Button button;

    private GameObject uiPanel;

    private CurrencyController currencyController;

    void Awake()
    {
        bandMemberController = FindObjectOfType<BandMemberController>();
        childText = transform.GetComponentInChildren<Text>();
        currencyController = FindObjectOfType<CurrencyController>();
        button = GetComponent<Button>();
        uiPanel = transform.GetChild(0).gameObject;
    }

    void OnEnable()
    {
        currencyController.OnCurrencyChanged += HandleCurrencyChanged;
    }

    void OnDisable()
    {
        currencyController.OnCurrencyChanged -= HandleCurrencyChanged;
    }

    void Start()
    {
        UpdateUpgradeButton();
    }

    public void OnUpgradeButtonClick()
    {
        if (CheckBuyable())
        {
            if (OnSkillUpgrade != null)
            {
                OnSkillUpgrade(this, new BandMemberSkillEventArgs(bandMember, bandMemberController.NextUpgrades[bandMember])); //TODO: check if upgrade can be initiated
            }
            UpdateUpgradeButton();
        }
    }

    private void HandleCurrencyChanged(object sender, CurrencyEventArgs e)
    {
        button.interactable = CheckBuyable();
    }

    private void UpdateUpgradeButton()
    {
        childText.text = String.Format("{0} Level: {1}, Cost: {2}", bandMember.ToString(), bandMemberController.NextUpgrades[bandMember].id, bandMemberController.NextUpgrades[bandMember].upgradeCost);
        button.interactable = CheckBuyable();
    }

    private bool CheckBuyable()
    {
        return bandMemberController.CanBuyNextUpgrade(bandMember);
    }

    public void SetUIActive(CharacterType characterType)
    {
        button.interactable = CheckBuyable();
        UpdateUpgradeButton();
        uiPanel.SetActive(characterType == bandMember);
    }
}
