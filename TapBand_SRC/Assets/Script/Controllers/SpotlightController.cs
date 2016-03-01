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
    private float actualCountDown;

    public SpotlightUI spotlightUI;
    public TapController tapController;

    public GameObject[] musicians;

    void Start()
    {
        SpotlightInterval = ReadFloat(GeneralProperties.SPOTLIGHT_INTERVAL);
        SpotlightMinDelay = ReadFloat(GeneralProperties.SPOTLIGHT_MIN_DELAY);
        SpotlightMaxDelay = ReadFloat(GeneralProperties.SPOTLIGHT_MAX_DELAY);
        SpotlightTapMultiplier = ReadFloat(GeneralProperties.SPOTLIGHT_TAP_MULTIPLIER);

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

    private float ReadFloat(string name)
    {
        return Convert.ToSingle(GameData.instance.FindGeneralDataByName(name).value);
    }
}
