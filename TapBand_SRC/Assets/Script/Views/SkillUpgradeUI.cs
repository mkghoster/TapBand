using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SkillUpgradeUI : MonoBehaviour
{
    public CharacterType bandMember;

    public GameObject uiPanel;
    public Button upgradeButton;
    public Text upgradeButtonText;

    private BackstageController backstageController;

    public void SetController(BackstageController controller)
    {
        backstageController = controller;
    }

    public void OnUpgradeButtonClick()
    {
        backstageController.UpdateCharacter(bandMember);        
    }

    public void UpdateUI()
    {
        upgradeButton.interactable = CheckBuyable();
        var nextUpgrade = backstageController.GetNextUpgrade(bandMember);
        upgradeButtonText.text = String.Format("{0} Level: {1}, Cost: {2}", bandMember.ToString(), nextUpgrade.id, nextUpgrade.upgradeCost);
    }

    private bool CheckBuyable()
    {
        return backstageController.CanBuyNextUpgrade(bandMember);
    }

    public void SetUIActive(CharacterType characterType)
    {
        UpdateUI();
        uiPanel.SetActive(characterType == bandMember);
    }
}
