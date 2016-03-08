using UnityEngine;
using System.Collections;


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
        //base.CreateSFXPool();
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


    void Update()
    {
        if (testBool)
        {
            TestFunc();
        }    
    }

    void TestFunc()
    {

       base.PlaySound();
    }

    void OnEnable()
    {
        songController.GiveEndOfSong += PlayMusicSound;
        concertController.EndOfConcert += StopMusicSoundConcert;
        concertController.RestartConcert += StopMusicSounds;
    }

    void OnDisable()
    {
        songController.GiveEndOfSong -= PlayMusicSound;
        concertController.EndOfConcert -= StopMusicSoundConcert;
        concertController.RestartConcert -= StopMusicSounds;
    }


    #region Music

    private void PlayMusicSound(SongData songData)
    {
        //id++: it's the previous song id
        actualIndex = CastSongIndex(songData.id + 1 );
      
        PlayMusicSoundUntilIndex(actualIndex);
    }

    //concert success
    void StopMusicSoundConcert(ConcertData concertData)
    {
        StopMusicSounds();
    }

    //concert fail
    void StopMusicSounds()
    {
        actualIndex = 0;
        PlayMusicSoundUntilIndex(actualIndex);
    }


    void PlayMusicSound(int index)
    {
        musicSources[index].loop = true;
        musicSources[index].volume = 1.0f;
        musicSources[index].Play();
    }

    void PlayMusicSoundUntilIndex(int index)
    {
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].Stop();
        }

        for (int i = 0; i <= index; i++)
        {
            musicSources[i].loop = true;
            musicSources[i].volume = 1.0f;
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

    #endregion


}
