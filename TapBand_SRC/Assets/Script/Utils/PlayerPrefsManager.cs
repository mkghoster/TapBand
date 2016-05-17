using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

   
    public static void SetMusicVolume(float volume)
    {
       PlayerPrefs.SetFloat(PlayerPrefsConsts.MUSIC_VOLUME_KEY, Mathf.Clamp(volume, 0.0f, 1.0f));
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(PlayerPrefsConsts.MUSIC_VOLUME_KEY, 0.5f);
    }

    public static void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(PlayerPrefsConsts.SFX_VOLUME_KEY, Mathf.Clamp( volume,0.0f,1.0f));
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(PlayerPrefsConsts.SFX_VOLUME_KEY, 0.5f);
    }

    //TODO: átrakni ezt a gamestate-be
    public static void SetEncoreSongTry(bool value)
    {
        if (value)
            PlayerPrefs.SetInt(PlayerPrefsConsts.ENCORE_SONG_TRY, 1);
        else
            PlayerPrefs.SetInt(PlayerPrefsConsts.ENCORE_SONG_TRY, 0);
    }

    public static bool GetEncoreSongTry()
    {
        return PlayerPrefs.GetInt(PlayerPrefsConsts.ENCORE_SONG_TRY, 0) == 0 ? false : true;
    }

    //TODO: ezt is a gamestatebe átrakni
    public static void SetPrevAudioConcertID(int id)
    {
        PlayerPrefs.SetInt(PlayerPrefsConsts.PREV_CONCERT_AUDIO_ID, id);
    }

    public static float GetPrevAudioConcertID()
    {
        return PlayerPrefs.GetInt(PlayerPrefsConsts.PREV_CONCERT_AUDIO_ID, 0);
    }


}
