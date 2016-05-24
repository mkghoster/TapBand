using UnityEngine;
using System.Collections;

public class ConcertController : MonoBehaviour
{
    private const float START_ENCORE_DELAY = 1f;//TODO: set the delay from param?

    private SongController songController;
    private TourController tourController;

    public event ConcertEvent OnConcertRestart;

    public event ConcertEvent OnConcertFinished;
    public event ConcertEvent OnConcertStarted;

    private ConcertData currentConcertData;
    private ConcertState concertState;

    public SongData CurrentSongData
    {
        get
        {
            return concertState.CurrentSong;
        }
    }

    public bool HasTriedEncore
    {
        get
        {
            return concertState.HasTriedEncore;
        }
    }

    public bool IsNextSongEncore
    {
        get
        {
            return concertState.CurrentSongIndex == 4; // good enough
        }
    }

    void Awake()
    {
        songController = (SongController)FindObjectOfType(typeof(SongController));
        tourController = (TourController)FindObjectOfType(typeof(TourController));

        concertState = GameState.instance.Concert;
        var concertDataList = GameData.instance.ConcertDataList;
        for (int i = 0; i < concertDataList.Count; i++)
        {
            if (concertDataList[i].id == concertState.CurrentConcertID)
            {
                currentConcertData = concertDataList[i];
                break;
            }
        }
    }

    void OnEnable()
    {
        songController.OnSongFinished += HandleSongFinished;

        tourController.RestartConcert += RestartConcertFromTheFirst;
    }
    void OnDisable()
    {
        songController.OnSongFinished -= HandleSongFinished;

        tourController.RestartConcert -= RestartConcertFromTheFirst;
    }

    void Start()
    {
        concertState.CurrentSong = currentConcertData.songList[concertState.CurrentSongIndex];
    }

    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        bool isEncoreSong = e.Data.isEncore;

        if (e.Status == SongStatus.Successful)
        {
            // If the encore song is successful, notify the world about the succesful concert, get the next concert, and update the state
            if (isEncoreSong)
            {
                OnConcertFinished(this, new ConcertEventArgs(currentConcertData, concertState));
                currentConcertData = GetNextConcert();
                concertState.ResetToConcert(currentConcertData);

                if (OnConcertStarted != null)
                {
                    OnConcertStarted(this, new ConcertEventArgs(currentConcertData, concertState));
                }
            }
            else
            {
                SongData nextSongData = GetNextSong(); // Ha itt null van, az para, rossz a táblázat. TODO: kéne kezelni
                concertState.LastCompletedSongID = e.Data.id;

                // The updated song id
                if (concertState.CurrentSong.isEncore)
                {
                    concertState.CurrentSong = null;
                    //TODO: pre-encore sequence (probably an event)
                    StartCoroutine(SetNextSongDelayed(START_ENCORE_DELAY, nextSongData));
                    concertState.HasTriedEncore = true;
                    PlayerPrefsManager.SetEncoreSongTry(true);
                }
                else
                {
                    SetNextSong(nextSongData);
                }
            }
        }
        else if (e.Status == SongStatus.Failed)
        {
            ResetToFirstSongOfConcert();
        }
        else if (e.Status == SongStatus.EncoreInitiated)
        {
            concertState.CurrentSong = null;
            StartCoroutine(SetNextSongDelayed(START_ENCORE_DELAY, currentConcertData.GetEncoreSongData()));
        }
    }

    //Returns the next song to play.
    private SongData GetNextSong()
    {
        if (currentConcertData.songList.Count <= concertState.CurrentSongIndex + 1)
        {
            return null;
        }
        else
        {
            return currentConcertData.songList[concertState.CurrentSongIndex + 1];
        }
    }

    // Sets the next song to play, and increments the current song index
    // This is required because of the pre-encore sequence
    private void SetNextSong(SongData nextSongData)
    {
        concertState.CurrentSongIndex++;
        concertState.CurrentSong = nextSongData;
    }

    private IEnumerator SetNextSongDelayed(float delay, SongData nextSongData)
    {
        yield return new WaitForSeconds(delay);
        SetNextSong(nextSongData);
    }

    private void ResetToFirstSongOfConcert()
    {
        if (OnConcertRestart != null)
        {
            OnConcertRestart(this, new ConcertEventArgs(currentConcertData, concertState));
        }
        concertState.ResetToConcert(currentConcertData);
    }

    private void RestartConcertFromTheFirst() // TODO: wat?
    {
        ConcertState state = GameState.instance.Concert;
        state.CurrentConcertID = GameData.instance.ConcertDataList[0].id;
        state.LastCompletedSongID = -1;
        state.CurrentSong = null;
    }

    private ConcertData GetNextConcert()
    {
        var concertDataList = GameData.instance.ConcertDataList;
        for (int i = 0; i < concertDataList.Count; i++)
        {
            if (concertDataList[i].id == currentConcertData.id)
            {
                if (concertDataList.Count > i + 1)
                {
                    return concertDataList[i + 1];
                }
            }
        }
        return currentConcertData; // if for some reason we couldn't find the next one, return the current one
    }
}
