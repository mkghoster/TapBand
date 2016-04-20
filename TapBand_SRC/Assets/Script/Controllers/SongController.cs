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
    public event ShowEncoreButtonEvent ShowEncoreEvent; 

    private TapController tapController;
    private TourController tourController;
    private HudUI hudUI;
    private SongData currentSong;
    private EncoreButton encoreButton;

    private float actualTapAmount = 0f;
    private float bossBattleCountDown = 0f;

    void Awake()
    {
        hudUI = (HudUI)FindObjectOfType(typeof(HudUI));
        tapController = (TapController)FindObjectOfType(typeof(TapController));
        tourController = (TourController)FindObjectOfType(typeof(TourController));
        encoreButton = (EncoreButton)FindObjectOfType(typeof(EncoreButton));
    }

    void OnEnable()
    {
        tapController.OnTap += IncomingTapStrength;
        tourController.RestartSong += ResetControllerState;
        hudUI.TapPassed += TapPassed;
        hudUI.TimePassed += TimePassed;
		hudUI.newSongData += GetSongData;
        //encoreButton.GiveEncoreButtonPressedEvent += StartEncoreSong();
    }

    void OnDisable()
    {
        tapController.OnTap -= IncomingTapStrength;
        tourController.RestartSong -= ResetControllerState;
        hudUI.TapPassed -= TapPassed;
        hudUI.TimePassed -= TimePassed;
		hudUI.newSongData -= GetSongData;
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
            if(CastSongIndex( currentSong.id) == 4 )
            {
                if (GiveRewardOfSong != null)
                {
                    GiveRewardOfSong(currentSong.coinReward);
                }
                ResetControllerState();
                WaitToBeginEncoreSong();


            }


            if (currentSong.tapGoal < actualTapAmount)
            {
				if(GiveEndOfSong != null)
				{
					GiveEndOfSong(currentSong);
				}
				if(GiveRewardOfSong != null)
				{
					GiveRewardOfSong(currentSong.coinReward);
				}
                ResetControllerState();
                currentSong = GiveNextSong();
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


    private void StartEncoreSong()
    {

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
