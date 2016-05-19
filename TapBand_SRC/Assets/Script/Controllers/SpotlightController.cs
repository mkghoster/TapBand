using UnityEngine;
using System.Collections;
using System;

public class SpotlightController : MonoBehaviour
{

    private float SpotlightInterval;
    private float SpotlightMinDelay;
    private float SpotlightMaxDelay;
    private float SpotlightTapMultiplier;

    private float initSpotlightCountdown;

    public SpotlightUI spotlightUI;
    public TapController tapController;

    public GameObject[] musicians;

    void Start()
    {
        SpotlightInterval = GameData.instance.GeneralData.SpotlightInterval;
        SpotlightMinDelay = GameData.instance.GeneralData.RandomMechanismMinDelay;//Fix this
        SpotlightMaxDelay = GameData.instance.GeneralData.RandomMechanismMaxDelay;
        SpotlightTapMultiplier = GameData.instance.GeneralData.SpotlightTapMultiplier;

        initSpotlightCountdown = CalculateAliveTime();

        spotlightUI.aliveTime = SpotlightInterval;

        tapController = FindObjectOfType<TapController>();
        tapController.SpotlightTapMultiplier = SpotlightTapMultiplier;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (initSpotlightCountdown <= 0)
        {
            int indexToActivate = UnityEngine.Random.Range(0, musicians.Length);
            spotlightUI.Activate(musicians[indexToActivate]);
            initSpotlightCountdown = CalculateAliveTime();
        } else
        {
            initSpotlightCountdown -= dt;
        }
    }

    private float CalculateAliveTime()
    {
        return UnityEngine.Random.Range(SpotlightMinDelay, SpotlightMaxDelay);
    }    
}
