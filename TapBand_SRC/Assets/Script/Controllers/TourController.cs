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
        restart.RestartEnabled += NewTourStartIsAvailable;
    }

    void OnDisable()
    {
        hud.NewTour -= CurrentTour;
        restart.NewLevel -= UpgradeLevel;
        restart.CurrentTour -= CurrentTour;
        restart.RestartEnabled -= NewTourStartIsAvailable;
    }

    private TourData CurrentTour()
    {
		return GameState.instance.Tour.CurrentTour;
	}

    private bool NewTourStartIsAvailable()
    {
        return GameState.instance.Currency.NumberOfFans >= GameState.instance.Tour.CurrentTour.fanRequirementToSkip;
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
