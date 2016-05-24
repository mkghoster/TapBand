using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SkillUpgradeUI : MonoBehaviour
{
    public CharacterType bandMember;
    public event BandMemberSkillEvent OnSkillUpgrade;

    private CharacterData unlockableSkill;
    private BandMemberController bandMemberController;

    private Text childText; //TODO: implement final ui 
    private Button button;

    private CurrencyController currencyController;

    void Awake()
    {
        bandMemberController = FindObjectOfType<BandMemberController>();
        childText = transform.GetComponentInChildren<Text>();
        currencyController = FindObjectOfType<CurrencyController>();
        button = GetComponent<Button>();
    }

    void Start()
    {
        unlockableSkill = bandMemberController.NextUpgrades[bandMember];
        UpdateText();
        CheckBuyable();
    }

    void OnEnable()
    {
        currencyController.OnCurrencyChanged += HandleCurrencyChanged;
        CheckBuyable();
    }

    void OnDisable()
    {
        currencyController.OnCurrencyChanged -= HandleCurrencyChanged;
    }

    public void OnUpgradeButtonClick()
    {
        if (OnSkillUpgrade != null)
        {
            OnSkillUpgrade(this, new BandMemberSkillEventArgs(bandMember, unlockableSkill)); //TODO: check if upgrade can be initiated
        }
        unlockableSkill = bandMemberController.NextUpgrades[bandMember]; // TODO: update UI (upgrading / upgraded?)
        UpdateText();
        CheckBuyable();
    }

    private void HandleCurrencyChanged(object sender, CurrencyEventArgs e)
    {
        CheckBuyable();
    }

    private void UpdateText()
    {
        childText.text = String.Format("{0} Level: {1}, Cost: {2}", bandMember.ToString(), unlockableSkill.id, unlockableSkill.upgradeCost);
    }

    private void CheckBuyable()
    {
        if (unlockableSkill == null)
        {
            button.interactable = false;
            return;
        }
        button.interactable = currencyController.CanBuyFromCoin(unlockableSkill.upgradeCost);
    }
}
