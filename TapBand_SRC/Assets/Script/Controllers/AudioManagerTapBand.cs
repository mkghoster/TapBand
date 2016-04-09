using UnityEngine;
//using System.Collections;

//TODO: - handle end of Tour!
//      - FadeOut at the end of the song

public class AudioManagerTapBand : AudioManager
{
    private string musicSoruceGameObjectPath = "AudioManager/MusicSources";
    private const int numberOfMusicBars = 5;

   
    private AudioSource[] musicSources;
    private GameObject musicSoruceGameObject; 
    
    private SongController songController;
    private ConcertController concertController;
    private TourController tourController;
    private ConcertState concertState;

    //index for Concert and MusicBars
    private int actualIndex;


    public bool testBool = false;

    void Awake()
    {
        songController = (SongController)GameObject.FindObjectOfType(typeof(SongController));
        concertController = (ConcertController)GameObject.FindObjectOfType(typeof(ConcertController));
        tourController = (TourController)GameObject.FindObjectOfType(typeof(TourController));

        musicSources = new AudioSource[numberOfMusicBars];


        musicSoruceGameObject = GameObject.Find(musicSoruceGameObjectPath);
        GetAllChildMusicSource();

        //inic SFX
        base.InitAudioManager();   
    }

    void Start()
    {
        concertState = GameState.instance.Concert;

        //first time
        MuteAndPlayAllMusicBars();

        //in the first play it will be 0, but that isn't a boss battle
        if (concertState.LastComplatedSongID != 0)
        {
            //it's the previous song id, need to inc
            actualIndex = CastSongIndex(concertState.LastComplatedSongID +1);
            FadeInMusicBarsUntilIndex(actualIndex);
        }
        else
        {
            FadeInMusicBarsUntilIndex(concertState.LastComplatedSongID);
        }
            
    }

    void OnEnable()
    {
        songController.GiveEndOfSong += EndOfSongEvent;
        concertController.EndOfConcert += EndOfConcertEvent;
        concertController.RestartConcert += ResetartConcert;
        tourController.OnPrestige += OnPrestigeEvent;
        tourController.RestartConcert += RestartConcertFromTour;  //TODO: ez kell?

        settingsUI.MusicVolumeChange += SetMusicVolume;
        settingsUI.SFXVolumeChange += SetSFXVolume;
    }

    void OnDisable()
    {
        songController.GiveEndOfSong -= EndOfSongEvent;
        concertController.EndOfConcert -= EndOfConcertEvent;
        concertController.RestartConcert -= ResetartConcert;
        tourController.OnPrestige -= OnPrestigeEvent;
        tourController.RestartConcert -= RestartConcertFromTour;

        settingsUI.MusicVolumeChange -= SetMusicVolume;
        settingsUI.SFXVolumeChange -= SetSFXVolume;  //base classban nem kapta meg az eventet, csak ha idehoztam
    }

    #region events
    private void EndOfSongEvent(SongData songData)
    {
        
        if (!songData.bossBattle)
        {
            //id++: it's the previous song id
            actualIndex = CastSongIndex(songData.id + 1);

            FadeInNextSong();
        }
        
    }

    //concert success
    void EndOfConcertEvent(ConcertData concertData)
    {
        StartNewOrPrevConcert(); 
    }

    //concert fail
    void ResetartConcert()
    {
        StartNewOrPrevConcert();
    }

    //OnPrestige
    void OnPrestigeEvent(TourData tourData)
    {
        StartNewOrPrevConcert();
    }

    //Nem kapott OnPrestigeEventet a torubol ha az első concerten nyomtunk ismét restart-ot
    void RestartConcertFromTour()
    {
        StartNewOrPrevConcert();
    }

    //Change to actual music bars volume
    void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        for (int i = 0; i <= actualIndex; i++)
        {
            musicSources[i].volume = musicVolume;
        }
    }

    void TestBars()
    {
        for (int i = 0; i < musicSources.Length; i++)
        {
            //print(i+":  "+musicSources[i].isPlaying);
            print(i + ":  " + musicSources[i].volume);
        }
    }

    #endregion

    //concert fail/succ -> same stuff, just from different events
    void StartNewOrPrevConcert()
    {
        StopMusicSounds(); 
        actualIndex = 0;
        MuteAndPlayAllMusicBars();
        FadeInMusicBarsUntilIndex(actualIndex);     
    }

    void StopMusicSounds()
    {
        StopAllCoroutines();
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].Stop();
        }
    }

    //called:  after every song
    void FadeInNextSong()
    {
        FadeClip(musicSources[actualIndex], FadeState.FadeIn);
    }

    //called when: game start, fail/succes concert
    void FadeInMusicBarsUntilIndex(int index)
    {
        for (int i = 0; i <= index; i++)
        {
            FadeClip(musicSources[i], FadeState.FadeIn);     
        }
    }


    //mute, loop and start all music bars
    void MuteAndPlayAllMusicBars()
    {
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].volume = 0.0f;
            musicSources[i].loop = true;
            musicSources[i].Play();
        }
    }


    //find && mute
    void GetAllChildMusicSource()
    {
        for (int i = 0; i < musicSoruceGameObject.transform.childCount; i++)
        {
            musicSources[i] = musicSoruceGameObject.transform.GetChild(i).GetComponent<AudioSource>();
            musicSources[i].volume = 0.0f;
        }
    }

    private int CastSongIndex(int songID)
    {
        int newID = (songID - 1) % numberOfMusicBars;
        return newID;
    }




}
