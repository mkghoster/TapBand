using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioManagerTapBand : AudioManager
{
    #region private fields
    private string musicSoruceGameObjectPath = "AudioManager/MusicSources";
    private const int numberOfMusicBars = 5;


    private AudioSource[] musicSources;
    private GameObject musicSoruceGameObject;

    private SongController songController;
    private ConcertController concertController;
    private TourController tourController;
    private ViewController viewController;

    private ConcertState concertState;

    //index for Concert and MusicBars
    private int actualIndex;

    private AudioClip[] clips;

    private AudioClip currentConcertEncoreLoopClip;
    private AudioClip currentConcertFirstClip;
    private bool needToPlayEncoeLoop;
    #endregion

    void Awake()
    {
        songController = (SongController)GameObject.FindObjectOfType(typeof(SongController));
        concertController = (ConcertController)GameObject.FindObjectOfType(typeof(ConcertController));
        tourController = (TourController)GameObject.FindObjectOfType(typeof(TourController));
        viewController = GameObject.FindObjectOfType<ViewController>();

        musicSources = new AudioSource[numberOfMusicBars];

        musicSoruceGameObject = GameObject.Find(musicSoruceGameObjectPath);
        GetAllChildMusicSource();

        //inic SFX
        base.InitAudioManager();


        ReadMusicFromResources();
        ChooseConcertAudio(true);
    }

    void Start()
    {
        concertState = GameState.instance.Concert;

        MuteAndPlayAllMusicBars();

        //in the first play it will be 0, but that isn't a boss battle
        if (concertState.LastCompletedSongID != 0)
        {
            //it's the previous song id, need to inc
            actualIndex = CastSongIndex(concertState.LastCompletedSongID + 1);

            //encore song : only the last bar need to fade in
            if (actualIndex == 4)
            {
                PlayEncoreSong();
            }
            else
            {
                FadeInMusicBarsUntilIndex(actualIndex + 1); // actualIndex + 1 : if it's 0  it doesn't fade anything
            }
        }
        else
        {
            FadeInMusicBarsUntilIndex(concertState.LastCompletedSongID);
        }

    }

    void Update()
    {
        if (concertController.CurrentConcertData != null && concertController.CurrentSongData.isEncore && musicSources[4].time >= musicSources[4].clip.length && !needToPlayEncoeLoop)
        {
            needToPlayEncoeLoop = true;
            StartEncoreLoopClip();
        }
    }


    void OnEnable()
    {
        songController.OnSongFinished += EndOfSongEvent;
        songController.OnSongStarted += SetUpEncoreLoopClip;

        concertController.OnConcertFinished += EndOfConcertEvent;
        concertController.OnConcertRestart += RestartConcert;
        tourController.OnPrestige += OnPrestigeEvent;
        tourController.RestartConcert += RestartConcertFromTour;

        settingsController.OnChangeSettingsToggle += MuteOnOfSound;
        viewController.OnViewChange += StopMusicByViewChange;
    }

    void OnDisable()
    {
        songController.OnSongFinished -= EndOfSongEvent;
        songController.OnSongStarted -= SetUpEncoreLoopClip;

        concertController.OnConcertFinished -= EndOfConcertEvent;
        concertController.OnConcertRestart -= RestartConcert;
        tourController.OnPrestige -= OnPrestigeEvent;
        tourController.RestartConcert -= RestartConcertFromTour;

        settingsController.OnChangeSettingsToggle -= MuteOnOfSound;
        viewController.OnViewChange -= StopMusicByViewChange;
    }

    #region events
    private void EndOfSongEvent(object sender, SongEventArgs e)
    {
        //id++: it's the previous song id
        actualIndex = CastSongIndex(e.Data.id + 1);

        //before the encore song
        if (actualIndex == 4)
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


    private void SetUpEncoreLoopClip(object sender, SongEventArgs e)
    {
        if (e.Data.isEncore)
            musicSources[0].clip = currentConcertEncoreLoopClip;
    }


    //concert success
    void EndOfConcertEvent(object sender, ConcertEventArgs e)
    {
        StartNewConcert();
    }

    //concert fail
    void RestartConcert(object sender, ConcertEventArgs e)
    {
        //switch back from encore loop clip
        musicSources[0].clip = currentConcertFirstClip;
        needToPlayEncoeLoop = false;

        StartPrevConcert();
    }

    //OnPrestige
    void OnPrestigeEvent()
    {
        StartNewConcert();
    }

    //Nem kapott OnPrestigeEventet a torubol ha az első concerten nyomtunk ismét restart-ot-----------???
    void RestartConcertFromTour()
    {
        StartNewConcert();
    }

    void MuteOnOfSound(object sender, SettingsEventArgs e)
    {
        if (e.Music)
        {
            musicVolume = 1.0f;
            SetMusicVolume(musicVolume);
            PlayerPrefsManager.SetMusicToggle(true);            
        }
        else
        {
            musicVolume = 0.0f;
            SetMusicVolume(musicVolume);
            PlayerPrefsManager.SetMusicToggle(false);
        }

        if (e.Sfx)
        {
            sfxVolume = 1.0f;
            PlayerPrefsManager.SetSFXToggle(true);
        }
        else
        {          
            sfxVolume = 0.0f;
            PlayerPrefsManager.SetSFXToggle(false);
        }
    }

    private void StopMusicByViewChange(object sender, ViewChangeEventArgs e)
    {
        switch (e.NewView)
        {
            case ViewType.STAGE:
                //legyen tanc
                ContinueMusicBars();
                break;
            case ViewType.BACKSTAGE:
                //stop music
                PauseMusicBars();
                break;
            case ViewType.DAILY_EVENT:
                //stop music
                break;
            case ViewType.CUSTOMIZATION:
                //stop music
                break;
        }
    }

    #endregion



    #region music bars handle methods

    void StartNewConcert()
    {
        StopMusicSounds();
        actualIndex = 0;

        ChooseConcertAudio(false);

        MuteAndPlayAllMusicBars();
        FadeInMusicBarsUntilIndex(actualIndex);
    }

    void StartPrevConcert()
    {
        StopMusicSounds();
        actualIndex = 0;

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
        for (int i = 0; i < musicSources.Length - 1; i++)
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
        if(index == 0)
        {
            FadeClip(musicSources[0], FadeState.FadeIn);
        }
        else
        {
            for (int i = 0; i < index; i++)
            {
                FadeClip(musicSources[i], FadeState.FadeIn);
            }
        }
        
    }

    //Play and Fade In encore
    void PlayEncoreSong()
    {
        musicSources[actualIndex].Play();
        FadeClip(musicSources[actualIndex], FadeState.FadeIn);
    }

    private void StartEncoreLoopClip()
    {
        if (needToPlayEncoeLoop)
        {
            musicSources[4].Stop();
            musicSources[4].volume = 0.0f;
            musicSources[0].volume = base.musicVolume;
            musicSources[0].Play();
            musicSources[0].loop = true;
        }
       
        
    }



    //mute, loop and start all music bars 
    void MuteAndPlayAllMusicBars()
    {
        //(except the last encore source)
        for (int i = 0; i < musicSources.Length - 1; i++)
        {
            musicSources[i].volume = 0.0f;
            musicSources[i].loop = true;
            musicSources[i].Play();
        }
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

    void PauseMusicBars()
    {
        if (actualIndex == 4)
            musicSources[actualIndex].Pause();
        else
        {
            for (int i = 0; i <= actualIndex; i++)
            {
                musicSources[i].Pause();
            }
        }
    }

    void ContinueMusicBars()
    {
        if (actualIndex == 4)
            musicSources[actualIndex].Play();
        else
        {
            for (int i = 0; i <= actualIndex; i++)
            {
                musicSources[i].Play();
            }
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

    #region Concert mix order

    private void ChooseConcertAudio(bool isStarted)
    {
        int[] correctAudioClipsOrderByConcert = { -1, -1, -1, -1, -1, -1 };
        int randomNumber = -1;

        if (isStarted)
            randomNumber = PlayerPrefsManager.GetPrevAudioConcertID();
        else
        {
            randomNumber = Random.Range(0, 6);
            while (randomNumber == PlayerPrefsManager.GetPrevAudioConcertID())
            {
                randomNumber = Random.Range(0, 6);
            }
        }


        switch (randomNumber)
        {
            case 0:
                correctAudioClipsOrderByConcert = FirstConcertOrderType();
                break;
            case 1:
                correctAudioClipsOrderByConcert = SecondConcertOrderType();
                break;
            case 2:
                correctAudioClipsOrderByConcert = ThirdConecertOrderType();
                break;
            case 3:
                correctAudioClipsOrderByConcert = FourthConcertOrderType();
                break;
            case 4:
                correctAudioClipsOrderByConcert = SecondConcertOrderType();
                break;
            case 5:
                correctAudioClipsOrderByConcert = SecondConcertOrderType();
                break;
            case 6:
                correctAudioClipsOrderByConcert = FifthConcertOrderType();
                break;
        }

        PlayerPrefsManager.SetPrevAudioConcertID(randomNumber);

        SetCorrectOrderAudioClips(correctAudioClipsOrderByConcert);

    }



    private void SetCorrectOrderAudioClips(int[] order)
    {
        int currentConcertAudioID = PlayerPrefsManager.GetPrevAudioConcertID();

        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].clip = clips[order[i] + (currentConcertAudioID * 6)];
        }

        //save the current concert encore loop clip
        currentConcertEncoreLoopClip = clips[order[order.Length - 1] + (currentConcertAudioID * 6)];
        currentConcertFirstClip = musicSources[0].clip;
    }

    private void ReadMusicFromResources()
    {
        var array = Resources.LoadAll("PlaceHolder", typeof(AudioClip));
        clips = new AudioClip[array.Length];
        for (int i = 0; i < clips.Length; i++)
        {
            clips[i] = array[i] as AudioClip;
        }
    }

    //0 - Guitar
    //1 - Drum
    //2 - Bass
    //3 - Synth
    //4 - Encore

    //music bar orders by gdd

    private int[] FirstConcertOrderType()
    {
        float n = Random.Range(0, 2);

        if (n % 3 == 0)
        {
            int[] order = { 0, 1, 2, 3, 4, 5 };
            return order;
        }
        else if (n % 3 == 1)
        {
            int[] order = { 1, 0, 2, 3, 4, 5 };
            return order;
        }
        else
        {
            int[] order = { 1, 2, 0, 3, 4, 5 };
            return order;
        }

    }

    private int[] SecondConcertOrderType()
    {
        float n = Random.Range(0, 2);

        if (n % 3 == 0)
        {
            int[] order = { 1, 2, 0, 3, 4, 5 };
            return order;
        }
        else if (n % 3 == 1)
        {
            int[] order = { 1, 0, 2, 3, 4, 5 };
            return order;
        }
        else
        {
            int[] order = { 2, 1, 0, 3, 4, 5 };
            return order;
        }
    }

    private int[] ThirdConecertOrderType()
    {


        float n = Random.Range(0, 3);

        if (n % 4 == 0)
        {
            int[] order = { 0, 1, 2, 3, 4, 5 };
            return order;
        }
        else if (n % 4 == 1)
        {
            int[] order = { 1, 2, 0, 3, 4, 5 };
            return order;
        }
        else if (n % 4 == 2)
        {
            int[] order = { 1, 0, 2, 3, 4, 5 };
            return order;
        }
        else
        {
            int[] order = { 2, 1, 0, 3, 4, 5 };
            return order;
        }

    }

    private int[] FourthConcertOrderType()
    {
        float n = Random.Range(0f, 1f);
        int[] order = new int[6];

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
        order[5] = 5;

        //print("order: " + order[0] + order[1] + order[2] + order[3] + order[4]);
        return order;
    }

    private int[] FifthConcertOrderType()
    {
        float n = Random.Range(0, 3);

        if (n % 4 == 0)
        {
            int[] order = { 0, 1, 2, 3, 4, 5 };
            return order;
        }
        else if (n % 4 == 1)
        {
            int[] order = { 1, 2, 0, 3, 4, 5 };
            return order;
        }
        else if (n % 4 == 2)
        {
            int[] order = { 1, 0, 2, 3, 4, 5 };
            return order;
        }
        else
        {
            int[] order = { 1, 2, 3, 0, 4, 5 };
            return order;
        }
    }

    #endregion


}
