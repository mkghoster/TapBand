using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

    private const string MUSIC_VOLUME_KEY = "music_volume";
    private const string SFX_VOLUME_KEY   = "sfx_volume";

    public static void SetMusicVolume(float volume)
    {
        if (volume >= 0.0f && volume <= 1.0f)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Music volume is out of range");
        }
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
    }

    public static void SetSFXVolume(float volume)
    {
        if (volume >= 0.0f && volume <= 1.0f)
        {
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("SFX volume is out of range");
        }
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
    }


}
