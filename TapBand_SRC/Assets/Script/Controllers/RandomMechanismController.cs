using UnityEngine;
using System.Collections;

public enum RandomMechanismType
{
    Spotlight = 0,
    Drone = 1
}

public class RandomMechanismController : MonoBehaviour
{
    #region Private fields
    private float minDelay;
    private float maxDelay;

    private float currentDelay;
    private float elapsedTime;

    private bool isMechanismActive;
    private bool isPaused = false;

    private ConcertController concertController;
    private SongController songController;
    private ViewController viewController;

    // This is a directly controlled by this controller. It will fire the correct events.
    private SpotlightController spotlightController;
    #endregion

    void Awake()
    {
        var generalData = GameData.instance.GeneralData;
        minDelay = generalData.RandomMechanismMinDelay;
        maxDelay = generalData.RandomMechanismMaxDelay;

        currentDelay = Random.Range(minDelay, maxDelay);

        concertController = FindObjectOfType<ConcertController>();
        songController = FindObjectOfType<SongController>();
        viewController = FindObjectOfType<ViewController>();

        spotlightController = FindObjectOfType<SpotlightController>();

        viewController.OnViewChange += ViewChanged;
    }

    void OnEnable()
    {
        spotlightController.OnSpotlightFinished += HandleRandomMechanismFinished;

        concertController.OnConcertFinished += HandleConcertFinished;
        songController.OnSongFinished += HandleSongFinished;
        songController.OnSongStarted += HandleSongStarted;
    }

    void OnDisable()
    {
        spotlightController.OnSpotlightFinished -= HandleRandomMechanismFinished;

        concertController.OnConcertFinished -= HandleConcertFinished;
        songController.OnSongFinished -= HandleSongFinished;
        songController.OnSongStarted -= HandleSongStarted;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused || isMechanismActive)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= currentDelay)
        {
            StartRandomMechanism();
            currentDelay = Random.Range(minDelay, maxDelay);
            elapsedTime = 0;
        }
    }

    private void StartRandomMechanism()
    {
        var mechanismToStart = SelectRandomMechanism();
        isMechanismActive = true;
        switch (mechanismToStart)
        {
            case RandomMechanismType.Spotlight:
                spotlightController.StartSpotlight();
                break;
            default:
                isMechanismActive = false;
                Debug.LogWarningFormat("RandomMechanismController: Starting {0} mechanism is not implemented", mechanismToStart);
                break;
        }
    }

    private RandomMechanismType SelectRandomMechanism()
    {
        //var mechanismSelector = Random.value;

        //if (mechanismSelector < 0.5)
        //{
        return RandomMechanismType.Spotlight;
        //}
        //else
        //{
        //    return RandomMechanismType.Drone;
        //}
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        spotlightController.SetPaused(paused);
        //TODO: pause drone
    }

    private void HandleRandomMechanismFinished(object sender, RandomMechanismEventArgs e)
    {
        isMechanismActive = false; // good enough        
    }

    private void HandleConcertFinished(object sender, ConcertEventArgs e)
    {
        // If the concert ends, end spotlights.
        // Pausing is handled by song events
        spotlightController.EndSpotlight();
    }

    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        SetPaused(true);
    }

    private void HandleSongStarted(object sender, SongEventArgs e)
    {
        SetPaused(false);
    }

    private void ViewChanged(object sender, ViewChangeEventArgs e)
    {
        SetPaused(e.NewView != ViewType.STAGE);
    }
}
