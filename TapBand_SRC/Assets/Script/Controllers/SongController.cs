using UnityEngine;
using System.Collections;

public class SongController : MonoBehaviour
{

    public delegate SongData GiveNextSongEvent();
    public event GiveNextSongEvent GiveNextSong;
    public event GiveNextSongEvent GiveFirstSongOfActualConcert;

    public delegate void GiveEndOfSongEvent(SongData songData);
    public event GiveEndOfSongEvent GiveEndOfSong;

    public delegate void GiveRewardOfSongEvent(int coinReward);
    public event GiveRewardOfSongEvent GiveRewardOfSong;

    public delegate void ShowEncoreButtonEvent();
    public event ShowEncoreButtonEvent ShowEncoreButton;

    //between 2 concert
    public delegate void SwitchOnOffTapEvent(bool value);
    public event SwitchOnOffTapEvent SwitchOnOffTap;


    private TapController tapController;
    private TourController tourController;
    private HudUI hudUI;
    private SongData currentSong;
    private EncoreButtonUI encoreButton;

    private float actualTapAmount = 0f;
    private float timeCountDown = 0f;
	private float bossBattleCountDownBooster = 0f;
	private bool extraTimeBoosterIsActive = false;

    // 3 because of currentsong always contains the previous song. We need the 4. song, ant it's previous is the 3.
    private const int beforeEncoreSongConstID = 3;

    //stop the boss time counter
    private bool isEncoreOver = false;

    

    void Awake()
    {
        hudUI = (HudUI)FindObjectOfType(typeof(HudUI));
        tapController = (TapController)FindObjectOfType(typeof(TapController));
        tourController = (TourController)FindObjectOfType(typeof(TourController));
        encoreButton = (EncoreButtonUI)FindObjectOfType(typeof(EncoreButtonUI));
    }

    void OnEnable()
    {
        tapController.OnTap += IncomingTapStrength;
        tourController.RestartSong += ResetControllerState;
        hudUI.TapPassed += TapPassed;
        hudUI.TimePassed += TimePassed;
        hudUI.newSongData += GetSongData;
        encoreButton.GiveEncoreButtonPressedEvent += StartEncoreSong;
    }

    void OnDisable()
    {
        tapController.OnTap -= IncomingTapStrength;
        tourController.RestartSong -= ResetControllerState;
        hudUI.TapPassed -= TapPassed;
        hudUI.TimePassed -= TimePassed;
        hudUI.newSongData -= GetSongData;
        encoreButton.GiveEncoreButtonPressedEvent -= StartEncoreSong;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        if (currentSong == null)
        {
            if (GiveNextSong != null)
            {
                currentSong = GiveNextSong();
            }    
        }

       
        if (!isEncoreOver)
            timeCountDown += deltaTime;
                                 
        if (currentSong.bossBattle)
        {
            /*if(!isEncoreOver)
                timeCountDown += deltaTime;*/
			if (extraTimeBoosterIsActive) {
                timeCountDown -= bossBattleCountDownBooster;
				extraTimeBoosterIsActive = false;
			}

            if (timeCountDown > currentSong.duration)
            {
                ResetartConcert();
            }        
        }
        else        //************************** ÁTMENETI   ********************************************************* amíg a nem boss songok hossza 0 !!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            if (timeCountDown > 20f) ///--------------- MOST BEÉGETVE A SIMA SZÁMOKNÁL!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            {
                ResetartConcert();
            }
        }

    }


    private void ResetartConcert()
    {
        ResetControllerState();
        currentSong = GiveFirstSongOfActualConcert();
    }

    private float TapPassed()
    {
        return actualTapAmount;
    }

    private float TimePassed()
    {
        return timeCountDown;
    }

    private void IncomingTapStrength(float tapStrength)
    {
        if (currentSong != null)
        {
            actualTapAmount += tapStrength;

            //last song before encore
            if (CastSongIndex(currentSong.id) == beforeEncoreSongConstID && currentSong.tapGoal < actualTapAmount)
            {
                //if there was already a try
                if (PlayerPrefsManager.GetEncoreSongTry())
                {
                    if (ShowEncoreButton != null)
                    {
                        ShowEncoreButton();
                    }
                }
                else
                {
                    PlayerPrefsManager.SetEncoreSongTry(true);          
                    StartNextSong();
                }

            }
            //else if, because we alawys do the same (except before the encore song)
            else if (currentSong.tapGoal < actualTapAmount)
            {
                //Succes boss battle: waiting and switch off the taparea
                if (currentSong.bossBattle)
                {
                    StartCoroutine(WaitAfterConcert(SongConcertTour.waitTimeBetweenConcerts));
                    if(SwitchOnOffTap != null)
                    {
                        SwitchOnOffTap(false);
                    }
                    isEncoreOver = true;
                }
                else
                {
                    StartNextSong();
                    isEncoreOver = false;
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
        timeCountDown = 0f;
        currentSong = null;
    }

    //ATMENETI!!!!!!!!!!!!!!
    public int GetSongID()
    {
        return currentSong.id;
    }

	public void BossExtratime(float extraTime){
		timeCountDown = -extraTime;
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
