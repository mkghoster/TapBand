using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//public class SettingsUI : MonoBehaviour {
public class SettingsController : MonoBehaviour
{
    //public delegate void SoundVolumeChangeEvent(float volume); ------- értesíteni az AudioManagert arról h némítva lett csak ennyi 
    //public event SoundVolumeChangeEvent MusicVolumeChange;
    //public event SoundVolumeChangeEvent SFXVolumeChange;

    //public Slider musicSlider;
    //public Slider sfxSlider;



    #region private fields
    private float defaultMusicVolume = 0.5f;
    private float defaultSFXVolume   = 0.5f;
    private ViewController viewController;
    private StageController stageController;
    private SettingsUI settingsUI;
    #endregion
    void Start () {

        //get listeners to the volume change
        //musicSlider.onValueChanged.AddListener(delegate { MusicValueChangeCheck(); });
        //sfxSlider.onValueChanged.AddListener(delegate { SFXValueChangeCheck(); });

        viewController = GameObject.FindObjectOfType<ViewController>();
        stageController = GameObject.FindObjectOfType<StageController>();
        settingsUI = GameObject.FindObjectOfType<SettingsUI>();

        stageController.OnSettingsButtonPressed += settingsUI.ShowUI;
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

    public void OnBackToGameClick()
    {
        viewController.EnterView(ViewType.BACKSTAGE);
    }
}
