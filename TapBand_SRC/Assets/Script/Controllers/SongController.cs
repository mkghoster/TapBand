using UnityEngine;
using System.Collections;

public class SongController : MonoBehaviour
{
    // Event to notify the world about the end of a song
    public event SongEvent OnSongFinished;

    // Event to notify the world about the start of a song
    public event SongEvent OnSongStarted;

    // Event to notify the world about the current song progress
    public event SongEvent OnSongProgress;

    public event SongEvent ShowEncoreButton; // TODO: this might be a concert event

    #region Private fields
    private ConcertController concertController;

    private TapController tapController;
    private TourController tourController;
    private SongData currentSong;
    private StageController stageController;
    private ViewController viewController;

    private double actualTapAmount = 0f;
    private float elapsedTime = 0f;

    private bool isSongPaused = false;

    // 3 because of currentsong always contains the previous song. We need the 4. song, ant it's previous is the 3.
    private const int beforeEncoreSongConstID = 3; //TODO: this still belongs to the concert
    #endregion

    void Awake()
    {
        concertController = FindObjectOfType<ConcertController>();

        tapController = FindObjectOfType<TapController>();
        tourController = FindObjectOfType<TourController>();

        stageController = FindObjectOfType<StageController>();

        viewController = FindObjectOfType<ViewController>();

        viewController.OnViewChange += ViewChanged;
    }

    void OnEnable()
    {
        tapController.OnTap += HandleTap;

        stageController.OnEncoreButtonPressed += StartEncoreSong;

        tourController.RestartSong += ResetControllerState;
    }

    void OnDisable()
    {
        tapController.OnTap -= HandleTap;
        
        stageController.OnEncoreButtonPressed -= StartEncoreSong;

        tourController.RestartSong -= ResetControllerState;
    }

    void Start()
    {
        currentSong = concertController.CurrentSongData;
        OnSongStarted(this, new SongEventArgs(currentSong, SongStatus.InProgress));
    }

    void Update()
    {
        if (isSongPaused)
        {
            return;
        }

        float deltaTime = Time.deltaTime;

        // try to get the next song from the concertController
        if (currentSong == null)
        {
            currentSong = concertController.CurrentSongData;
            // if we didn't get any (for example concert changing is in progress, do nothing)
            if (currentSong == null)
            {
                return;
            }
            else
            {
                if (OnSongStarted != null)
                {
                    OnSongStarted(this, new SongEventArgs(currentSong, SongStatus.InProgress, 0, 0));
                }
            }
        }

        // if we have finished the current song, notify the world about this, and reset the controller
        if (actualTapAmount >= currentSong.tapGoal)
        {
            if (OnSongFinished != null)
            {
                OnSongFinished(this, new SongEventArgs(currentSong, SongStatus.Successful));
            }
            ResetControllerState();
            return;
        }

        elapsedTime += deltaTime;

        if (elapsedTime > currentSong.duration)
        {
            FailSong();
            return;
        }

        if (OnSongProgress != null)
        {
            OnSongProgress(this, new SongEventArgs(currentSong, SongStatus.InProgress, actualTapAmount, elapsedTime));
        }
    }


    private void FailSong()
    {
        if (OnSongFinished != null)
        {
            OnSongFinished(this, new SongEventArgs(currentSong, SongStatus.Failed));
        }
        ResetControllerState();
    }

    private void HandleTap(object sender, TapEventArgs e)
    {
        if (currentSong != null)
        {
            actualTapAmount += e.TapStrength;

            //last song before encore
            //TODO: this should be in it's own handler
            if (CastSongIndex(currentSong.id) == beforeEncoreSongConstID && currentSong.tapGoal < actualTapAmount)
            {
                //if there was already a try
                if (PlayerPrefsManager.GetEncoreSongTry())
                {
                    if (ShowEncoreButton != null)
                    {
                        ShowEncoreButton(null, null);
                    }
                }
                else
                {
                    PlayerPrefsManager.SetEncoreSongTry(true);
                    if (currentSong.isEncore)
                    {
                        PlayerPrefsManager.SetEncoreSongTry(false);
                    }

                    ResetControllerState();
                }
            }
            //else if, because we alawys do the same (except before the encore song)
        }
    }

    private void ResetControllerState()
    {
        actualTapAmount = 0f;
        elapsedTime = 0f;
        currentSong = null;
    }

    //TODO: ezt eventből kéne megkapni
    public void BossExtratime(float extraTime)
    {
        elapsedTime -= extraTime;
    }


    //when we push the encore button
    private void StartEncoreSong()
    {
        if (OnSongFinished != null)
        {
            OnSongFinished(this, new SongEventArgs(currentSong, SongStatus.EncoreInitiated));
        }

        PlayerPrefsManager.SetEncoreSongTry(true); //TODO: ez concertstate lesz

        ResetControllerState();
    }

    private int CastSongIndex(int songID)
    {
        int newID = (songID - 1) % 5;
        return newID;
    }

    public void HandleSongPaused(bool paused)
    {
        isSongPaused = paused;
    }

    private void ViewChanged(object sender, ViewChangeEventArgs e)
    {
        HandleSongPaused(e.NewView != ViewType.STAGE);
    }
}
