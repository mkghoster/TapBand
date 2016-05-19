﻿using UnityEngine;
using System.Collections;
using System;
using System.Linq;

[System.Serializable]
public class ConcertState
{

    private int currentConcertID;
    private int lastComplatedSongID;
    [NonSerialized]
    private SongData currentSong;



    public int CurrentConcertID
    {
        get
        {
            return currentConcertID;
        }
        set
        {
            currentConcertID = value;
        }
    }

    public int LastComplatedSongID
    {
        get
        {
            return lastComplatedSongID;
        }
        set
        {
            lastComplatedSongID = value;
        }
    }


    //geci gány, proto jó lesz
    public SongData GetNextSong()
    {
        return GameData.instance.SongDataList.FirstOrDefault(x => x.id == lastComplatedSongID + 1);
    }

    public ConcertData GetNextConcert()
    {
        return GameData.instance.ConcertDataList.FirstOrDefault(x => x.id == currentConcertID + 1);
    }

    public SongData CurrentSong
    {
        get
        {
            return currentSong;
        }
        set
        {
            currentSong = value;
        }
    }

    public ConcertData CurrentConcert
    {
        get
        {
            return GameData.instance.ConcertDataList.FirstOrDefault(x => x.id == currentConcertID);
        }
    }


}
