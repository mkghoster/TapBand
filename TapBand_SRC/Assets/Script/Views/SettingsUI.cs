using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour {

    public GameObject settingsPanel;
    public Toggle sfxToggle;
    public Toggle musicToggle;

    #region private fields
    private SettingsController settingsController;
    #endregion

    void Awake()
    {
        settingsController = GameObject.FindObjectOfType<SettingsController>();
    }

    void Start()
    {
        musicToggle.isOn = PlayerPrefsManager.GetMusicToggle();
        sfxToggle.isOn = PlayerPrefsManager.GetSFXToggle();

        /*print("pp music: "+PlayerPrefsManager.GetMusicToggle());
        print("pp sfx:   "+PlayerPrefsManager.GetSFXToggle());
        print("---");
        print("sfxtoggle: " + sfxToggle.isOn);
        print("musictoggle: " + musicToggle.isOn);*/
    }

    public void OnBackToGameButtonClick()
    {
        HideUI();
        settingsController.OnBackToGameClick();     
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
        settingsController.OnPressToggle( musicToggle.isOn, sfxToggle.isOn );
    }
    public void SwitchSFXMuteButton()
    {
        settingsController.OnPressToggle( musicToggle.isOn, sfxToggle.isOn );
    }

}
