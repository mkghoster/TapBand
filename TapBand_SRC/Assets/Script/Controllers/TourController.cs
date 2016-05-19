using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void RestartEvent();
public delegate void OnPrestigeEvent();

public class TourController : MonoBehaviour
{
    public event RestartEvent RestartConcert;
    public event RestartEvent RestartSong;
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
        if (RestartConcert != null)
        {
            RestartConcert();
        }
        if (RestartSong != null)
        {
            RestartSong();
        }
        if (OnPrestige != null)
        {
            OnPrestige();
        }
    }
}
