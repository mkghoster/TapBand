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

    private TapController tapController;
    private TourController tourController;
    private HudUI hudUI;
    private SongData currentSong;

    private float actualTapAmount = 0f;
    private float bossBattleCountDown = 0f;

    void Awake()
    {
        hudUI = (HudUI)FindObjectOfType(typeof(HudUI));
        tapController = (TapController)FindObjectOfType(typeof(TapController));
        tourController = (TourController)FindObjectOfType(typeof(TourController));
    }

    void OnEnable()
    {
        tapController.OnTap += IncomingTapStrength;
        tourController.RestartSong += ResetControllerState;
        hudUI.TapPassed += TapPassed;
        hudUI.TimePassed += TimePassed;
		hudUI.newSongData += GetSongData;
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
}
