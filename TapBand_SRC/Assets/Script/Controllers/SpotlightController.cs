using UnityEngine;
using System.Collections;
using System;

public class SpotlightController : MonoBehaviour
{
    private float spotlightInterval;
    private float spotlightChangeInterval;

    private bool isActive;
    private float timeToLive;
    private float timeToChange;

    private SpotlightUI spotlightUI;
    private SongController songController;

    private bool isPaused;

    public event RandomMechanismEvent OnSpotlightFinished;

    void Awake()
    {
        var generalData = GameData.instance.GeneralData;

        songController = FindObjectOfType<SongController>();
        spotlightUI = FindObjectOfType<SpotlightUI>();

        spotlightInterval = generalData.SpotlightInterval;
        spotlightChangeInterval = generalData.SpotlightChangeInterval;
    }

    void Update()
    {
        if (!isActive || isPaused)
        {
            return;
        }
        var dt = Time.deltaTime;
        timeToLive -= dt;
        timeToChange -= dt;

        if (timeToLive <= 0)
        {
            EndSpotlight();
        }
        if (timeToChange <= 0)
        {
            spotlightUI.ChangeSpotlight();
            timeToChange = spotlightChangeInterval;
        }


    }

    public void StartSpotlight()
    {
        timeToLive = spotlightInterval;
        timeToChange = spotlightChangeInterval;
        isActive = true;

        spotlightUI.ChangeSpotlight();
    }

    private void EndSpotlight()
    {
        isActive = false;
        spotlightUI.DeactivateAll();
        if (OnSpotlightFinished != null)
        {
            OnSpotlightFinished(this, new RandomMechanismEventArgs(RandomMechanismType.Spotlight));
        }
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }
}
