using UnityEngine;
using System.Collections;

public class SongController : MonoBehaviour {

    public delegate SongData GiveNextSongEvent();
    public event GiveNextSongEvent GiveNextSong;

    private TapController tapController;
    private HudUI hudUI;
    private SongData currentSong;

    private float actualTapAmount = 0f;

    void Awake()
    {
        hudUI = (HudUI)FindObjectOfType(typeof(HudUI));
        tapController = (TapController)FindObjectOfType(typeof(TapController));
    }

    void OnEnable()
    {
        tapController.OnTap += IncomingTapStrength;
        hudUI.NewSong += GetSongName;
    }

    void OnDisable()
    {
        tapController.OnTap -= IncomingTapStrength;
        hudUI.NewSong -= GetSongName;
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
            }
        }
    }

    private string GetSongName()
    {
        return currentSong == null ? "" : currentSong.title;
    }
}
