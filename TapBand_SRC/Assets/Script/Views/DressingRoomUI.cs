using UnityEngine;
using System.Collections;
using System;

public class DressingRoomUI : MonoBehaviour
{
    public SkillUpgradeUI bassUI;
    public SkillUpgradeUI drumsUI;
    public SkillUpgradeUI guitar1UI;
    public SkillUpgradeUI guitar2UI;
    public SkillUpgradeUI keyboardsUI;

    private CharacterType currentCharacter;

    private GameObject dressingRoomPanel;

    private BackstageController backstageController;

    void Awake()
    {
        currentCharacter = CharacterType.Bass;
        dressingRoomPanel = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        dressingRoomPanel.SetActive(false);
    }

    public void SetController(BackstageController controller)
    {
        backstageController = controller;

        bassUI.SetController(controller);
        drumsUI.SetController(controller);
        guitar1UI.SetController(controller);
        guitar2UI.SetController(controller);
        keyboardsUI.SetController(controller);
    }

    public void OnCharacterRightClick()
    {
        var currentIndex = (int)currentCharacter;
        currentIndex++;
        if (currentIndex > 4)
        {
            currentIndex = 0;
        }
        else if (currentIndex < 0)
        {
            currentIndex = 4;
        }

        SwitchCharacterPanel((CharacterType)currentIndex);
    }

    public void OnCharacterLeftClick()
    {
        var currentIndex = (int)currentCharacter;
        currentIndex--;
        if (currentIndex > 4)
        {
            currentIndex = 0;
        }
        else if (currentIndex < 0)
        {
            currentIndex = 4;
        }

        SwitchCharacterPanel((CharacterType)currentIndex);
    }

    public void OnBackstageButtonClick()
    {
        HideUI();
        backstageController.SwitchDressingRoom(false);
    }

    private void SwitchCharacterPanel(CharacterType character)
    {
        bassUI.SetUIActive(character);
        drumsUI.SetUIActive(character);
        guitar1UI.SetUIActive(character);
        guitar2UI.SetUIActive(character);
        keyboardsUI.SetUIActive(character);

        currentCharacter = character;
    }

    public void ShowUI()
    {
        SwitchCharacterPanel(currentCharacter);
        dressingRoomPanel.SetActive(true);
    }

    private void HideUI()
    {
        dressingRoomPanel.SetActive(false);
    }

    public void UpdateUI()
    {
        bassUI.UpdateUI();
        drumsUI.UpdateUI();
        guitar1UI.UpdateUI();
        guitar2UI.UpdateUI();
        keyboardsUI.UpdateUI();
    }
}
