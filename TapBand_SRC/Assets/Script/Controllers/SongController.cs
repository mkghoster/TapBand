using UnityEngine;
using System.Collections;

public class SongController : MonoBehaviour {

    public delegate SongData GiveNextSongEvent();
    public event GiveNextSongEvent GiveNextSong;
    public event GiveNextSongEvent GiveFirstSongOfActualConcert;

	public delegate void GiveEndOfSongEvent(SongData songData);
	public event GiveEndOfSongEvent GiveEndOfSong;

	public delegate void GiveRewardOfSongEvent(int coinReward);
	public event GiveRewardOfSongEvent GiveRewardOfSong;

    public delegate void ShowEncoreButtonEvent();
    public event ShowEncoreButtonEvent ShowEncoreButton; 

    private TapController tapController;
    private TourController tourController;
    private HudUI hudUI;
    private SongData currentSong;
    private EncoreButtonUI encoreButton;

    private float actualTapAmount = 0f;
    private float bossBattleCountDown = 0f;

    // private bool wasEncoreSongTry = false; 

    // 3 because of currentsong always contains the previous song. We need the 4. song, ant it's previous is the 3.
    private const int beforeEncoreSongConstID = 3;

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

        if (currentSong.bossBattle)
        {
            bossBattleCountDown += deltaTime;

            if (bossBattleCountDown > currentSong.duration)
            {
                ResetControllerState();
                currentSong = GiveFirstSongOfActualConcert();
            }
        }
        //print("wasEncoreSongTry:" + PlayerPrefsManager.GetEncoreSongTry());
    }
	
    private float TapPassed()
    {
        return actualTapAmount;
    }

    private float TimePassed()
    {
        return bossBattleCountDown;
    }

    private void IncomingTapStrength(float tapStrength)
    {
        if (currentSong != null)
        {
            actualTapAmount += tapStrength;

            //last song before encore
            if(CastSongIndex( currentSong.id) == beforeEncoreSongConstID   &&  currentSong.tapGoal < actualTapAmount)  
            {
                //if there was already a try
                if(PlayerPrefsManager.GetEncoreSongTry())
                {
                    //WaitToBeginEncoreSong();   //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ez most kell eeeee???????????????????????
                    if(ShowEncoreButton != null)
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
            else  if (currentSong.tapGoal < actualTapAmount) 
            {
                StartNextSong();
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
        bossBattleCountDown = 0f;
        currentSong = null;
    }

    //ATMENETI!!!!!!!!!!!!!!
    public int GetSongID()
    {
        return currentSong.id;
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


    IEnumerator WaitToBeginEncoreSong()
    {
        yield return new WaitForSeconds(10f);
    }


    private int CastSongIndex(int songID)
    {
        int newID = (songID - 1) % 5;
        return newID;
    }
}
