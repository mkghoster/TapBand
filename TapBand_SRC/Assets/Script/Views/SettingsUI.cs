using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsUI : MonoBehaviour {

    public delegate void SoundVolumeChangeEvent(float volume);
    public event SoundVolumeChangeEvent MusicVolumeChange;
    public event SoundVolumeChangeEvent SFXVolumeChange;

    public Slider musicSlider;
    public Slider sfxSlider;

    private float defaultMusicVolume = 0.5f;
    private float defaultSFXVolume   = 0.5f;

    void Start () {

        //get listeners to the volume change
        musicSlider.onValueChanged.AddListener(delegate { MusicValueChangeCheck(); });
        sfxSlider.onValueChanged.AddListener(delegate { SFXValueChangeCheck(); });

       
        //Init volumes from preferences
        musicSlider.value = PlayerPrefsManager.GetMusicVolume();
        sfxSlider.value = PlayerPrefsManager.GetSFXVolume();
    }
	
	
    private void MusicValueChangeCheck()
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
            
    }


    public void SetAllToDefault()
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
    }
}
