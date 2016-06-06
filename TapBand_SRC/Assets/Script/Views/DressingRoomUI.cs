using UnityEngine;
using System.Collections;
using System;

public class DressingRoomUI : MonoBehaviour
{
    public GameObject bassUI;
    public GameObject drumsUI;
    public GameObject guitar1UI;
    public GameObject guitar2UI;
    public GameObject keyboardsUI;

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
    }

    private void SwitchCharacterPanel(CharacterType character)
    {
        var bassUIActive = false;
        var drumsUIActive = false;
        var guitar1UIActive = false;
        var guitar2UIActive = false;
        var keyboardsUIActive = false;

        switch (character)
        {
            case CharacterType.Bass:
                bassUIActive = true;
                break;
            case CharacterType.Drums:
                drumsUIActive = true;
                break;
            case CharacterType.Guitar1:
                guitar1UIActive = true;
                break;
            case CharacterType.Guitar2:
                guitar2UIActive = true;
                break;
            case CharacterType.Keyboards:
                keyboardsUIActive = true;
                break;
            default:
                throw new NotImplementedException("Unknown character type");
        }

        bassUI.SetActive(bassUIActive);
        drumsUI.SetActive(drumsUIActive);
        guitar1UI.SetActive(guitar1UIActive);
        guitar2UI.SetActive(guitar2UIActive);
        keyboardsUI.SetActive(keyboardsUIActive);

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
}
