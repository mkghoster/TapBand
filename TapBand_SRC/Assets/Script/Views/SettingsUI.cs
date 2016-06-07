using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour {

    public GameObject settingsPanel;

    public void OnBackToGameButtonClick()
    {
        HideUI();
    }

    public void ShowUI()
    {
        settingsPanel.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        settingsPanel.gameObject.SetActive(false);
    }

    public void SwitchMusicMuteButton()
    {

    }
    public void SwitchSFXMuteButton()
    {

    }

}
