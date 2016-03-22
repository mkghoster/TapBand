using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

    private const string MUSIC_VOLUME_KEY = "music_volume";
    private const string SFX_VOLUME_KEY   = "sfx_volume";

    public static void SetMusicVolume(float volume)
    {
       PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, Mathf.Clamp(volume, 0.0f, 1.0f));
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.5f);
    }

    public static void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, Mathf.Clamp( volume,0.0f,1.0f));
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.5f);
    }


}
