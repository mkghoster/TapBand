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
    private SongController songController;

    public GameObject[] musicians;

    private bool canActivate = true;

    void Awake()
    {
        songController = (SongController)FindObjectOfType(typeof(SongController));
    }

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

    void OnEnable()
    {
        songController.SwitchOnOffTap += SwitchOnOffSpotlight;
    }

    void OnDisable()
    {
        songController.SwitchOnOffTap -= SwitchOnOffSpotlight;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (canActivate)
        {
            if (initSpotlightCountdown <= 0)
            {
                int indexToActivate = UnityEngine.Random.Range(0, musicians.Length);
                spotlightUI.Activate(musicians[indexToActivate]);
                initSpotlightCountdown = CalculateAliveTime();
            }
            else
            {
                initSpotlightCountdown -= dt;
            }
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

    //between 2 concert
    private void SwitchOnOffSpotlight(bool value)
    {
        canActivate = value;
    }
}
