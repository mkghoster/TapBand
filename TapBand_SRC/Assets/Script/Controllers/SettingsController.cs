using UnityEngine;
using System.Collections;

public class SettingsController : MonoBehaviour
{
    #region private fields
    private float defaultMusicVolume = 0.5f;
    private float defaultSFXVolume   = 0.5f;
    private ViewController viewController;
    private BackstageController backstageController;
    private SettingsUI settingsUI;
    #endregion

    public event SettingsEvent OnChangeSettingsToggle;




    void Start () {
        viewController = GameObject.FindObjectOfType<ViewController>();
        backstageController = GameObject.FindObjectOfType<BackstageController>();
        settingsUI = GameObject.FindObjectOfType<SettingsUI>();

        backstageController.OnSettingsButtonPressed += settingsUI.ShowUI;
        //Init volumes from preferences
       
        //musicSlider.value = PlayerPrefsManager.GetMusicVolume();
        //sfxSlider.value = PlayerPrefsManager.GetSFXVolume();
    }
	
	
    /*private void MusicValueChangeCheck()
    {
        if (MusicVolumeChange != null)
        {
            MusicVolumeChange(musicSlider.value);
            PlayerPrefsManager.SetMusicVolume(musicSlider.value);
        }
            
    }

    private void SFXValueChangeCheck()
    {
        if (SFXVolumeChange != null)
        {
            SFXVolumeChange(sfxSlider.value);
            PlayerPrefsManager.SetSFXVolume(sfxSlider.value);
        }
            
    }*/


    /*public void SetAllToDefault()
    {
        if(MusicVolumeChange != null)
        {
            MusicVolumeChange(defaultMusicVolume);
            PlayerPrefsManager.SetMusicVolume(defaultMusicVolume);
            musicSlider.value = defaultMusicVolume;
        }

        if(SFXVolumeChange != null)
        {
            SFXVolumeChange(defaultSFXVolume);
            PlayerPrefsManager.SetSFXVolume(defaultSFXVolume);
            sfxSlider.value = defaultSFXVolume;
        }
    }*/

    public void OnPressToggle(bool musicToggle, bool sfxToogle)
    {
        if (OnChangeSettingsToggle != null)
        {
            OnChangeSettingsToggle(this, new SettingsEventArgs(musicToggle, sfxToogle));
        }
    }

   
    

    public void OnBackToGameClick()
    {
        viewController.EnterView(ViewType.BACKSTAGE);
    }
}
