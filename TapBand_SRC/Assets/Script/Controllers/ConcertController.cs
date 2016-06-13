using UnityEngine;
using System.Collections;
using System;

public class ConcertController : MonoBehaviour
{
    private const float START_ENCORE_DELAY = 1f;//TODO: set the delay from param?

    private SongController songController;
    private TourController tourController;
    private StageController stageController;


    public event ConcertEvent OnConcertRestart;

    public event ConcertEvent OnConcertFinished;
    public event ConcertEvent OnConcertStarted;

    private ConcertData currentConcertData;
    private ConcertState concertState;

    public event Action ShowEncoreButton;
    public event Action HideEncoreButton;


    private const int beforeEncoreSongConstID = 4;

    public SongData CurrentSongData
    {
        get
        {
            return concertState.CurrentSong;
        }
    }

    public ConcertData CurrentConcertData
    {
        get
        {
            return currentConcertData;
        }
    }

    public bool HasTriedEncore//------------------ kell e???
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
        stageController = (StageController)FindObjectOfType(typeof(StageController));

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

        stageController.OnEncoreButtonPressed += HandleEncoreButtonPressed;
    }
    void OnDisable()
    {
        songController.OnSongFinished -= HandleSongFinished;

        tourController.RestartConcert -= RestartConcertFromTheFirst;

        stageController.OnEncoreButtonPressed -= HandleEncoreButtonPressed;
    }

    void Start()
    {
        concertState.CurrentSong = currentConcertData.songList[concertState.CurrentSongIndex];

        if (concertState.HasTriedEncore && CastSongIndex( CurrentSongData.id ) == beforeEncoreSongConstID)
        {
            if (ShowEncoreButton != null)
            {
                ShowEncoreButton();
            }
        }
        else
        {
            if (HideEncoreButton != null)
            {
                HideEncoreButton();
            }
        }
    }


    private void HandleEncoreButtonPressed()
    {
        //eldinítani az encoert


        //elrejteni az encort gombot
        if(HideEncoreButton != null)
        {
            HideEncoreButton();
        }
    }

    private void ActivateEncoreButton() //TODO: rename
    {
        if(  CastSongIndex( CurrentSongData.id ) == beforeEncoreSongConstID  &&  concertState.HasTriedEncore )
        {
            if(ShowEncoreButton != null)
            {
                ShowEncoreButton();
            }
        }

        //last song before encore
        //TODO: this should be in it's own handler
        /*if (CastSongIndex(currentSong.id) == beforeEncoreSongConstID && currentSong.tapGoal < actualTapAmount)
        {
            //if there was already a try
            //if (PlayerPrefsManager.GetEncoreSongTry())
            if (GameState.instance.Concert.HasTriedEncore)
            {
                if (ShowEncoreButton != null)
                {
                    ShowEncoreButton(this, null);
                }
            }
            else
            {
                //PlayerPrefsManager.SetEncoreSongTry(true);
                GameState.instance.Concert.HasTriedEncore = true;
                if (currentSong.isEncore)
                {
                    //PlayerPrefsManager.SetEncoreSongTry(false);
                    GameState.instance.Concert.HasTriedEncore = false;
                }

                ResetControllerState();
            }
        }*/
        //if()  ha encore, 


    }

   /* private void DeactivateEncoreButton()
    {
        //alapjáraton elindulásnál megvuzsglni h kell e mutatni
    }*/

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
                ActivateEncoreButton();

                SongData nextSongData = GetNextSong(); // Ha itt null van, az para, rossz a táblázat. TODO: kéne kezelni
                concertState.LastCompletedSongID = e.Data.id;

                // The updated song id
                if (concertState.CurrentSong.isEncore)
                {
                    concertState.CurrentSong = null;
                    //TODO: pre-encore sequence (probably an event)
                    StartCoroutine(SetNextSongDelayed(START_ENCORE_DELAY, nextSongData));
                    concertState.HasTriedEncore = true;
                    //PlayerPrefsManager.SetEncoreSongTry(true);
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
       
        SetTheFirstConcertAfterPrestige();

    }

    //TODO: meghatrozni normalisan a feltelt és a tapStrenghtet
    private void SetTheFirstConcertAfterPrestige()
    {
       /* double actualTapStrength = 1.1; 

        var concertDataList = GameData.instance.ConcertDataList;
        for(int i = 0; i < GameData.instance.ConcertDataList.Count; i++)
        {
            print(i+".: levelRange: "+ concertDataList[i].levelRange);
            if ( ( concertDataList[i].levelRange / 100 ) < actualTapStrength)
            {
                if( i  ==  0)  //ne legyen negativ
                {
                    i++;
                }
                print("i: "+i);
                concertState.ResetToConcert(GameData.instance.ConcertDataList[i - 1]);
                currentConcertData = GameData.instance.ConcertDataList[i - 1];
                //break;
            }
        }*/

        //TEMP
        concertState.ResetToConcert(GameData.instance.ConcertDataList[0]);
        currentConcertData = GameData.instance.ConcertDataList[0];
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

    private int CastSongIndex(int songID)
    {
        int newID = (songID - 1) % 5;
        return newID;
    }
}
