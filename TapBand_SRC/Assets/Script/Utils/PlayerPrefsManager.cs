﻿using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour
{
    public static void SetMusicToggle(bool value)
    {
        if (value)
            PlayerPrefs.SetInt(PlayerPrefsConsts.MUSIC_TOGGLE, 1);
        else
            PlayerPrefs.SetInt(PlayerPrefsConsts.MUSIC_TOGGLE, 0);
    }

    public static bool GetMusicToggle()
    {
        return PlayerPrefs.GetInt(PlayerPrefsConsts.MUSIC_TOGGLE, 0) == 0 ? false : true;
    }

    public static void SetSFXToggle(bool value)
    {
        if (value)
            PlayerPrefs.SetInt(PlayerPrefsConsts.SFX_TOGGLE, 1);
        else
            PlayerPrefs.SetInt(PlayerPrefsConsts.SFX_TOGGLE, 0);
    }

    public static bool GetSFXToggle()
    {
        return PlayerPrefs.GetInt(PlayerPrefsConsts.SFX_TOGGLE, 0) == 0 ? false : true;
    }


    //*********

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

    public static int GetPrevAudioConcertID()
    {
        return PlayerPrefs.GetInt(PlayerPrefsConsts.PREV_CONCERT_AUDIO_ID, 0);
    }


    //TODO: Debug ------------------------------------------
    public static void SetDebugTapMultip(float value)
    {
        //PlayerPrefs.SetFloat(PlayerPrefsConsts.SFX_VOLUME_KEY, Mathf.Clamp(volume, 0.0f, 1.0f));
        PlayerPrefs.SetFloat(PlayerPrefsConsts.DEBUG_TAP_MULTIP, value);
    }

    public static float GetDebugTapMultip()
    {
        return PlayerPrefs.GetFloat(PlayerPrefsConsts.DEBUG_TAP_MULTIP, 1f);
    }
}
