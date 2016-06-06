using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour {

    public GameObject settingsPanel;

    #region private fields
    private SettingsController settingsController;
    #endregion


    void Start () {
	
	}
	
    public void SetController(SettingsController controller)
    {
        this.settingsController = controller;
    }

    public void BakcToGameButtinClic()
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
