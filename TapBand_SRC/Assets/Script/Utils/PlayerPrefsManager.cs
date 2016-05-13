using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

    private const string MUSIC_VOLUME_KEY = "music_volume";
    private const string SFX_VOLUME_KEY   = "sfx_volume";
    private const string ENCORE_SONG_TRY = "encore_song_try";

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

    public static void SetEncoreSongTry(bool value)
    {
        if (value)
            PlayerPrefs.SetInt(ENCORE_SONG_TRY, 1);
        else
            PlayerPrefs.SetInt(ENCORE_SONG_TRY, 0);
    }

    public static bool GetEncoreSongTry()
    {
        return PlayerPrefs.GetInt(ENCORE_SONG_TRY, 0) == 0 ? false : true;
    }

}
