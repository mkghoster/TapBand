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
    private ConcertState concertState;

    //index for Concert and MusicBars
    private int actualIndex;


    public bool testBool = false;

    void Awake()
    {
        songController = (SongController)GameObject.FindObjectOfType(typeof(SongController));
        concertController = (ConcertController)GameObject.FindObjectOfType(typeof(ConcertController));

        musicSources = new AudioSource[numberOfMusicBars];


        musicSoruceGameObject = GameObject.Find(musicSoruceGameObjectPath);
        GetAllChildMusicSource();

        //inic SFX
        base.InitAudioManager();   
    }

    void Start()
    {
        concertState = GameState.instance.Concert;
        
        //in the first play it will be 0, but that isn't a boss battle
        if (concertState.LastComplatedSongID != 0)
        {
            //it's the previous song id, need to inc
            actualIndex = CastSongIndex(concertState.LastComplatedSongID +1); 
            PlayMusicSoundUntilIndex(actualIndex);
        }
        else
        {
            PlayMusicSoundUntilIndex(concertState.LastComplatedSongID);
        }
            
    }

    void OnEnable()
    {
        songController.GiveEndOfSong += PlayMusicSound;
        concertController.EndOfConcert += StopMusicSoundConcert;
        concertController.RestartConcert += ResetartConcert;

        settingsUI.MusicVolumeChange += SetMusicVolume;
        settingsUI.SFXVolumeChange += SetSFXVolume;
    }

    void OnDisable()
    {
        songController.GiveEndOfSong -= PlayMusicSound;
        concertController.EndOfConcert -= StopMusicSoundConcert;
        concertController.RestartConcert -= ResetartConcert;

        settingsUI.MusicVolumeChange -= SetMusicVolume;
        settingsUI.SFXVolumeChange -= SetSFXVolume;  //base classban nem kapta meg az eventet, csak ha idehoztam
    }

    #region events
    private void PlayMusicSound(SongData songData)
    {
        //id++: it's the previous song id
        actualIndex = CastSongIndex(songData.id + 1 );

        //PlayMusicSoundUntilIndex(actualIndex);
        PlayNextSong();
    }

    //concert success
    void StopMusicSoundConcert(ConcertData concertData)
    {
        StartNewOrPrevConcert();
    }

    //concert fail
    void ResetartConcert()
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

    #endregion

    //concert fail/succ -> same stuff, just from different events
    void StartNewOrPrevConcert()
    {
        StopMusicSounds();
        actualIndex = 0;
        PlayMusicSoundUntilIndex(actualIndex);
    }

    void StopMusicSounds()
    {
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].volume = 0f;
            musicSources[i].Stop();
        }
    }

    //called:  after every song
    void PlayNextSong()
    {
        musicSources[actualIndex].loop = true;
        FadeClip(musicSources[actualIndex], FadeState.FadeIn);
        //FadeClip(musicSources[actualIndex], FadeState.FadeOut);
        musicSources[actualIndex].Play();
    }

    //called when: fail/succes concert, game start
    void PlayMusicSoundUntilIndex(int index)
    {
        for (int i = 0; i <= index; i++)
        {
            musicSources[i].loop = true;
            FadeClip(musicSources[i], FadeState.FadeIn);
            musicSources[i].Play();       
        }
    }


    

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
