using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public delegate void RestartEvent();
public delegate void OnPrestigeEvent();

public class TourController : MonoBehaviour
{
    public event RestartEvent RestartConcert;
    public event RestartEvent RestartSong;
    public event OnPrestigeEvent OnPrestige;

    private RestartUI restart;

    private CurrencyController currencyController;
    private ConcertController concertController;

    void Awake()
    {
        restart = FindObjectOfType<RestartUI>();

        currencyController = FindObjectOfType<CurrencyController>();
        concertController = FindObjectOfType<ConcertController>();
    }

    void OnEnable()
    {
        restart.NewLevel += UpgradeLevel;
    }

    void OnDisable()
    {
        restart.NewLevel -= UpgradeLevel;
    }

    //TODO: rename
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
        if (OnPrestige != null)     //TODO: Reward átbeszélse
        {
            OnPrestige();
        }       
    }
}
