using UnityEngine;
using System.Collections;
using System;

public class DressingRoomUI : MonoBehaviour
{
    private GameObject bassUI;
    private GameObject drumsUI;
    private GameObject guitar1UI;
    private GameObject guitar2UI;
    private GameObject keyboardsUI;

    private CharacterType currentCharacter;

    void Awake()
    {
        currentCharacter = CharacterType.Bass;
        bassUI = transform.FindChild("Bass").gameObject;
        drumsUI = transform.FindChild("Drums").gameObject;
        guitar1UI = transform.FindChild("Guitar1").gameObject;
        guitar2UI = transform.FindChild("Guitar2").gameObject;
        keyboardsUI = transform.FindChild("Keyboards").gameObject;
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
