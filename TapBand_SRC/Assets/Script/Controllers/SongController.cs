using UnityEngine;
using System.Collections;

public class SongController : MonoBehaviour
{
    public event SongEvent OnSongFinished;
    public event SongEvent OnSongStarted;

    public event SongEvent ShowEncoreButton; // TODO: this might be a concert event

    private ConcertController concertController;

    private TapController tapController;
    private TourController tourController;
    private HudUI hudUI;
    private SongData currentSong;
    private EncoreButtonUI encoreButton;

    private float actualTapAmount = 0f;
    private float elapsedTime = 0f;
    private float bossBattleCountDownBooster = 0f;
    private bool extraTimeBoosterIsActive = false;

    // 3 because of currentsong always contains the previous song. We need the 4. song, ant it's previous is the 3.
    private const int beforeEncoreSongConstID = 3; //TODO: this still belongs to the concert

    void Awake()
    {
        concertController = (ConcertController)FindObjectOfType(typeof(ConcertController));

        hudUI = (HudUI)FindObjectOfType(typeof(HudUI));
        tapController = (TapController)FindObjectOfType(typeof(TapController));
        tourController = (TourController)FindObjectOfType(typeof(TourController));

        encoreButton = (EncoreButtonUI)FindObjectOfType(typeof(EncoreButtonUI));
    }

    void OnEnable()
    {
        tapController.OnTap += HandleTap;
        tourController.RestartSong += ResetControllerState;
        hudUI.newSongData += GetSongData;
        encoreButton.GiveEncoreButtonPressedEvent += StartEncoreSong;
    }

    void OnDisable()
    {
        tapController.OnTap -= HandleTap;
        tourController.RestartSong -= ResetControllerState;

        hudUI.newSongData -= GetSongData;
        encoreButton.GiveEncoreButtonPressedEvent -= StartEncoreSong;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        // try to get the next song from the concertController
        if (currentSong == null)
        {
            currentSong = concertController.GetNextSong();
            // if we didn't get any (for example concert changing is in progress, do nothing)
            if (currentSong == null)
            {
                return;
            }
            else
            {
                if (OnSongStarted != null)
                {
                    OnSongStarted(this, new SongEventArgs(currentSong, SongStatus.InProgress);
                }
            }
        }
        

        elapsedTime += deltaTime;


        if (extraTimeBoosterIsActive) //ez fos, boostercontrollernek kéne ideszólni
        {
            elapsedTime -= bossBattleCountDownBooster;
            extraTimeBoosterIsActive = false;
        }

        if (elapsedTime > currentSong.duration)
        {
            FailSong();
        }
    }


    private void FailSong()
    {
        ResetControllerState();
        if (OnSongFinished != null)
        {
            OnSongFinished(this, new SongEventArgs(currentSong, SongStatus.Failed));
        }
    }

    private void HandleTap(float tapStrength)
    {
        if (currentSong != null)
        {
            actualTapAmount += tapStrength;

            //last song before encore
            //TODO: why is this in the tap handler?
            if (CastSongIndex(currentSong.id) == beforeEncoreSongConstID && currentSong.tapGoal < actualTapAmount)
            {
                //if there was already a try
                if (PlayerPrefsManager.GetEncoreSongTry()) //wtf?
                {
                    if (ShowEncoreButton != null)
                    {
                        ShowEncoreButton(null, null); // todo: concert stuff still?
                    }
                }
                else
                {
                    PlayerPrefsManager.SetEncoreSongTry(true);
                    StartNextSong();
                }
            }
            //else if, because we alawys do the same (except before the encore song)

            if (actualTapAmount >= currentSong.tapGoal)
            {
                if (OnSongFinished != null)
                {
                    OnSongFinished(this, new SongEventArgs(currentSong, SongStatus.Successful));
                }
                //Succes boss battle: waiting and switch off the taparea
                if (currentSong.bossBattle)
                {                    
                }
                else
                {
                 
                }


            }
        }
    }

    private string GetSongName()
    {
        return currentSong == null ? "" : currentSong.title;
    }

    private SongData GetSongData()
    {
        return currentSong;
    }

    private void ResetControllerState()
    {
        actualTapAmount = 0f;
        elapsedTime = 0f;
        currentSong = null;
    }

    //ATMENETI!!!!!!!!!!!!!!
    public int GetSongID()
    {
        return currentSong.id;
    }

    public void BossExtratime(float extraTime)
    {
        elapsedTime = -extraTime;
        extraTimeBoosterIsActive = true;
    }

    //when we push the encore button
    private void StartEncoreSong()
    {
        StartNextSong();
    }

    private void StartNextSong()
    {
        if (GiveEndOfSong != null)
        {
            GiveEndOfSong(currentSong);
            //when we complete the encore song : reset the try
            if (currentSong.bossBattle)
            {
                PlayerPrefsManager.SetEncoreSongTry(false);
            }
        }
        if (GiveRewardOfSong != null)
        {
            GiveRewardOfSong(currentSong.coinReward);
        }
        ResetControllerState();
        currentSong = GiveNextSong();
    }


    private int CastSongIndex(int songID)
    {
        int newID = (songID - 1) % 5;
        return newID;
    }

    //wait and after switch on the TapArea collider and give next song
    private IEnumerator WaitAfterConcert(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //visszakapcsolás
        if (SwitchOnOffTap != null)
        {
            SwitchOnOffTap(true);
        }
        isEncoreOver = false;
        StartNextSong();
    }
}
