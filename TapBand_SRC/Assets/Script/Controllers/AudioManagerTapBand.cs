using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//TODO: - settingsUI: eventek folyamatosan jönnek (akkor is ha nem állítom a csúszkát )  --> más megoldás? (ezért volt az encore song közben mindegyik sáv maxon szólt)   
//  - cast indexet utilsba átemelni és mindenhova azt hívni ne helyit!!!!


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

    //private int prevConcertAudioID; // kmenetni az előző concert audio id-t és ellenőrizni  volt e 

    private AudioClip[] clips;


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


        ReadMusicFromResources();
        ChooseConcertAudio();
    }

    void Start()
    {
        concertState = GameState.instance.Concert;

        MuteAndPlayAllMusicBars();

        //in the first play it will be 0, but that isn't a boss battle
        if (concertState.LastComplatedSongID != 0)
        {
            //it's the previous song id, need to inc
            actualIndex = CastSongIndex(concertState.LastComplatedSongID +1);
            
            //encore song : only the last bar need to fade in
            if (actualIndex == 4)  
            {
                PlayEncoreSong();
            }
            else
            {
                FadeInMusicBarsUntilIndex(actualIndex);
            }             
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
        tourController.RestartConcert += RestartConcertFromTour;

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
        //id++: it's the previous song id
        actualIndex = CastSongIndex(songData.id + 1);

        //before the encore song
        if(actualIndex == 4)
        {
            FadeOutPreviousBars();

            StartCoroutine(WaitUntilFadeOutBeforEncore());

            PlayEncoreSong();
        }
        else
        {
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
        if (actualIndex == 4)
            musicSources[actualIndex].volume = musicVolume;
        else
        {
            for (int i = 0; i <= actualIndex; i++)
            {
                musicSources[i].volume = musicVolume;
            }
        }
       
    }

    #endregion



    #region music bars handle methods

    //concert fail/succ -> same stuff, just from different events
    void StartNewOrPrevConcert()
    {
        StopMusicSounds(); 
        actualIndex = 0;

        ChooseConcertAudio();

        MuteAndPlayAllMusicBars();
        FadeInMusicBarsUntilIndex(actualIndex);     
    }

    void StopMusicSounds()
    {
        StopAllCoroutines();//----???
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].Stop();
        }
    }

    //at the begin of the encore song
    void FadeOutPreviousBars()
    {
        for (int i = 0; i < musicSources.Length-1; i++)
        {
            FadeClip(musicSources[i], FadeState.FadeOut);
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

    //Play and Fade In encore
    void PlayEncoreSong()
    {
        musicSources[actualIndex].Play();
        FadeClip(musicSources[actualIndex], FadeState.FadeIn);
    }



    //mute, loop and start all music bars 
    void MuteAndPlayAllMusicBars()
    {
        //(except the last encore source)
        for (int i = 0; i < musicSources.Length-1; i++)
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

    #endregion

    //0 == encore song
    //1 == first song, ..., 4 == before encore song
    private int CastSongIndex(int songID)
    {
        int newID = (songID - 1) % numberOfMusicBars;
        return newID;
    }

    private IEnumerator WaitUntilFadeOutBeforEncore()
    {
        yield return new WaitForSeconds(base.fadeDuration);
    }


    /*void TestBars()
  {
      print("-----------------");
      for (int i = 0; i < musicSources.Length; i++)
      {
          //print(i+":  "+musicSources[i].isPlaying);
          print(i + ":  " + musicSources[i].volume);
      }
      print("*******************");
  }*/

    #region Concert mix order

    private void ChooseConcertAudio() //átnevezeni
    {
        int[] ret = { -1,-1,-1,-1,-1 };

        

        int randomNumber = Random.Range(0, 3);

        while(randomNumber == PlayerPrefsManager.GetPrevAudioConcertID())
        {
            randomNumber = Random.Range(0,3);
        }

        switch (randomNumber)
        {
            case 0:
                print("0");               
                ret = FirstConcertOrder();
                break;
            case 1:
                print("1");              
                ret = SecondConcertOrder();
                break;
            case 2:
                print("2");            
                ret = SecondConcertOrder();
                break;
            case 3:
                print("3");               
                ret = ThirdConecertOrder();
                break;
        }

        PlayerPrefsManager.SetPrevAudioConcertID(randomNumber);

        /*for(int i = 0; i < ret.Length; i++)
        {
            print(i + ": "+ ret[i]);
        }*/

        SetCorrectOrderAudioClips(ret);

    }

    //0 - Guitar
    //1 - Drum
    //2 - Bass
    //3 - Synth
    //4 - Encore
    
    private void SetCorrectOrderAudioClips( int[] order)
    {
        int currentConcertAudioID = PlayerPrefsManager.GetPrevAudioConcertID();

        for(int i = 0; i< order.Length;i++)
        {
            musicSources[i].clip = clips[ order[i] + (currentConcertAudioID * 5) ];
        }
    }

    private void ReadMusicFromResources()
    {
       //--------------------------------------------------------------------- TODO, try catchbe
        var array = Resources.LoadAll("PlaceHolder", typeof(AudioClip));
        clips = new AudioClip[array.Length];
        for (int i = 0; i < clips.Length; i++)
        {
            clips[i] = array[i] as AudioClip;
        }
    
        /*for(int i = 0; i < clips.Length; i++)
        {
            print("name: "+ clips[i].name);
        }*/

    }


    //Happy Develeopers
    private int[] FirstConcertOrder()
    {
        float n = Random.Range(0f,1f);
        int[] order = new int[5];
        if (n >= 0.5f)
            order[0] = 0;
        else
            order[0] = 1;


        n = Random.Range(0f,1f);
        if (order[0] == 1)
            order[1] = 0;
        else if ( order[0] == 0 && n <= 0.5f)
            order[1] = 1;
        else
            order[1] = 2;


        if (order[0] == 0 && order[1] == 2)
            order[2] = 1;
        else
            order[2] = 2;

        order[3] = 3;
        order[4] = 4;

        print("order: "+ order[0] + order[1] + order[2] + order[3] + order[4]);
        return order;

    }

    private int[] SecondConcertOrder()
    {
        float n = Random.Range(0,2);
        //int("1. n: "+ n);
        int[] order = new int[5];

        if (n % 3 == 0)
            order[0] = 0;
        else if (n % 3 == 1)
            order[0] = 1;
        else
            order[0] = 2;

        n = Random.Range(0f, 1f);
        if (order[0] == 1 || order[0] == 2)
            order[1] = 0;
        else if (order[0] == 0 && n >= 0.5f)
            order[1] = 1;
        else if (order[0] == 0 && n <= 0.5f)
            order[1] = 2;

        if ((order[0] == 0 && order[1] == 2) || (order[0] == 2 && order[1] == 0))  //----------TODO: előző kettőben a fordított eset lekezeése
            order[2] = 1;
        else   
            order[2] = 2;

        order[3] = 3;
        order[4] = 4;


        print("order: " + order[0] + order[1] + order[2] + order[3] + order[4]);
        return order;
    }

    private int[] ThirdConecertOrder()
    {
        float n = Random.Range(0f, 1f);
        int[] order = new int[5];

        if (n >= 0.5f)
            order[0] = 1;
        else
            order[0] = 2;

        if (order[0] == 1)
            order[1] = 2;
        else
            order[1] = 1;

        order[2] = 0;
        order[3] = 3;
        order[4] = 4;

        print("order: " + order[0] + order[1] + order[2] + order[3] + order[4]);
        return order;
    }


    #endregion


}
