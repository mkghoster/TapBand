using UnityEngine;
using System.Collections;

public enum RandomMechanismType
{
    Spotlight = 0,
    Drone = 1
}

public class RandomMechanismController : MonoBehaviour
{
    private float minDelay;
    private float maxDelay;

    private float nextStartTime;

    private bool isMechanismActive;
    private bool isPaused = false;

    // This is a directly controlled by this controller. It will fire the correct events.
    private SpotlightController spotlightController;

    void Awake()
    {
        var generalData = GameData.instance.GeneralData;
        minDelay = generalData.RandomMechanismMinDelay;
        maxDelay = generalData.RandomMechanismMaxDelay;

        nextStartTime = Random.Range(minDelay, maxDelay);

        spotlightController = FindObjectOfType<SpotlightController>();
    }

    void OnEnable()
    {
        spotlightController.OnSpotlightFinished += HandleRandomMechanismFinished;
    }

    void OnDisable()
    {
        spotlightController.OnSpotlightFinished -= HandleRandomMechanismFinished;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused || isMechanismActive)
        {
            return;
        }

        if (Time.time >= nextStartTime)
        {
            StartRandomMechanism();
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

    private void SetPaused(bool paused)
    {
        isPaused = paused;
        spotlightController.SetPaused(paused);
        //TODO: pause drone
    }

    private void HandleRandomMechanismFinished(object sender, RandomMechanismEventArgs e)
    {
        isMechanismActive = false; // good enough
        nextStartTime = Time.time + Random.Range(minDelay, maxDelay);
    }
}
