using UnityEngine;
using System.Collections;

public class SongController : MonoBehaviour {

    public delegate SongData GiveNextSongEvent();
    public event GiveNextSongEvent GiveNextSong;
    public event GiveNextSongEvent GiveFirstSongOfActualConcert;

    private TapController tapController;
    private HudUI hudUI;
    private SongData currentSong;

    private float actualTapAmount = 0f;
    private float bossBattleCountDown = 0f;

    void Awake()
    {
        hudUI = (HudUI)FindObjectOfType(typeof(HudUI));
        tapController = (TapController)FindObjectOfType(typeof(TapController));
    }

    void OnEnable()
    {
        tapController.OnTap += IncomingTapStrength;
        hudUI.NewSong += GetSongName;
        hudUI.TapPassed += TapPassed;
        hudUI.TimePassed += TimePassed;
    }

    void OnDisable()
    {
        tapController.OnTap -= IncomingTapStrength;
        hudUI.NewSong -= GetSongName;
        hudUI.TapPassed -= TapPassed;
        hudUI.TimePassed -= TimePassed;
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
                currentSong = GiveFirstSongOfActualConcert();
                actualTapAmount = 0f;
                bossBattleCountDown = 0f;
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
                currentSong = GiveNextSong();
                actualTapAmount = 0f;
                bossBattleCountDown = 0f;
            }
        }
    }

    private string GetSongName()
    {
        return currentSong == null ? "" : currentSong.title;
    }
}
