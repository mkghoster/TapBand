using UnityEngine;
using System.Collections;
using System;
using System.Linq;

[System.Serializable]
public class ConcertState
{
    // The current concert id
    public int CurrentConcertID { get; set; }

    // The current song id
    public int LastCompletedSongID { get; set; }

    // The current song index in the current concert
    public int CurrentSongIndex { get; set; }

    // Has the player failed on the encore song yet?
    public bool HasTriedEncore { get; set; }

    // The current song
    [NonSerialized]
    private SongData currentSong;
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

    // Resets the state to the given concert
    public void ResetToConcert(ConcertData concertData)
    {
        CurrentConcertID = concertData.id;
        LastCompletedSongID = -1;
        CurrentSongIndex = 0;
        currentSong = concertData.songList[0];
        HasTriedEncore = false; //---------------------- ez így nem biztos h jó
    }
}
