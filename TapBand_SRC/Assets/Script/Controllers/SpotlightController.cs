using UnityEngine;
using System.Collections;
using System;

public class SpotlightController : MonoBehaviour
{
    public event RandomMechanismEvent OnSpotlightFinished;

    #region Private fields
    private float spotlightInterval;
    private float spotlightChangeInterval;

    private bool isActive;
    private float timeToLive;
    private float timeToChange;

    private SpotlightUI spotlightUI;

    private bool isPaused;

    private ViewController viewController;
    #endregion

    void Awake()
    {
        var generalData = GameData.instance.GeneralData;

        spotlightUI = FindObjectOfType<SpotlightUI>();

        viewController = FindObjectOfType<ViewController>();

        viewController.OnViewChange += ViewChanged;

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

    public void EndSpotlight()
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

    private void ViewChanged(object sender, ViewChangeEventArgs e)
    {
        SetPaused(e.NewView != ViewType.STAGE);
    }
}
