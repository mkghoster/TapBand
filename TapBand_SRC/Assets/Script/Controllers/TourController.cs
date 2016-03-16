using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TourController : MonoBehaviour {

    public delegate void RestartEvent();
    public event RestartEvent RestartConcert;
    public event RestartEvent RestartSong;
    public delegate void OnPrestigeEvent(TourData tour);
    public event OnPrestigeEvent OnPrestige;

    private RestartUI restart;
    private HudUI hud;

    void Awake()
    {
        restart = FindObjectOfType<RestartUI>();
        hud = FindObjectOfType<HudUI>();
    }

    void OnEnable()
    {
        restart.NewLevel += UpgradeLevel;
    }

    void OnDisable()
    {
        restart.NewLevel -= UpgradeLevel;
    }

    private void UpgradeLevel()
    {
        if (RestartConcert != null)RestartConcert();
        if (RestartSong != null) RestartSong();
        if (OnPrestige != null)
        {
            int fans = GameState.instance.Currency.Fans;
            foreach (TourData tourData in GameData.instance.TourDataList)
            {
                if (fans > tourData.minFanCount)
                {
                    OnPrestige(tourData);
                    break;
                }
            }
            
        }
    }
}
