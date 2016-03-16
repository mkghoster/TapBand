using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TourController : MonoBehaviour {

    public delegate void RestartEvent();
    public event RestartEvent RestartConcert;
    public event RestartEvent RestartSong;
    public event RestartEvent ResetCurrencies;

    private HudUI hud;
    private RestartUI restart;

    void Awake()
    {
        hud = (HudUI)FindObjectOfType(typeof(HudUI));
        restart = (RestartUI)FindObjectOfType(typeof(RestartUI));
    }

    void OnEnable()
    {
        hud.NewTour += CurrentTour;
        restart.NewLevel += UpgradeLevel;
        restart.CurrentTour += CurrentTour;
    }

    void OnDisable()
    {
        hud.NewTour -= CurrentTour;
        restart.NewLevel -= UpgradeLevel;
        restart.CurrentTour -= CurrentTour;
    }

    private TourData CurrentTour()
    {
		return GameState.instance.Tour.CurrentTour;
	}
    
    private TourData NextTour()
    {
        return ListUtils.NextOf(GameData.instance.TourDataList, GameState.instance.Tour.CurrentTour);
    }

    private void UpgradeLevel()
    {
        GameState.instance.Tour.CurrentTourIndex += 1;
        if (RestartConcert != null)RestartConcert();
        if (RestartSong != null) RestartSong();
        if (ResetCurrencies != null) ResetCurrencies();
    }
}
