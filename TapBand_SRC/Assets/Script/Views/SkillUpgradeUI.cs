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

    void Awake()
    {
        bandMemberController = FindObjectOfType<BandMemberController>();
        childText = transform.GetComponentInChildren<Text>();
    }

    void Start()
    {
        unlockableSkill = bandMemberController.NextUpgrades[bandMember];
        UpdateText();
    }

    public void OnUpgradeButtonClick()
    {
        if (OnSkillUpgrade != null)
        {
            OnSkillUpgrade(this, new BandMemberSkillEventArgs(bandMember, unlockableSkill)); //TODO: check if upgrade can be initiated
        }
        unlockableSkill = bandMemberController.NextUpgrades[bandMember]; // TODO: update UI (upgrading / upgraded?)
        UpdateText();
    }

    private void UpdateText()
    {
        childText.text = String.Format("Level: {0}, Cost: {1}", unlockableSkill.id, unlockableSkill.upgradeCost);
    }
}
