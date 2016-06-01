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

    void Awake()
    {
        currentCharacter = CharacterType.Bass;
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

        SwitchCharacter((CharacterType)currentIndex);
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

        SwitchCharacter((CharacterType)currentIndex);
    }

    private void SwitchCharacter(CharacterType switchTo)
    {
        SetCharacterPanelActive(currentCharacter, false);

        currentCharacter = switchTo;
        SetCharacterPanelActive(switchTo, true);
    }

    private void SetCharacterPanelActive(CharacterType character, bool isActive)
    {
        switch (character)
        {
            case CharacterType.Bass:
                bassUI.SetActive(isActive);
                break;
            case CharacterType.Drums:
                drumsUI.SetActive(isActive);
                break;
            case CharacterType.Guitar1:
                guitar1UI.SetActive(isActive);
                break;
            case CharacterType.Guitar2:
                guitar2UI.SetActive(isActive);
                break;
            case CharacterType.Keyboards:
                keyboardsUI.SetActive(isActive);
                break;
            default:
                throw new NotImplementedException("Unknown character type");
        }
    }
}
