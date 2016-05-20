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

    // private bool canActivate = true;
    private bool canActivate = false; // TODO: ideiglenesen kikapcsolva

    void Awake()
    {
        songController = (SongController)FindObjectOfType(typeof(SongController));
    }

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

    void OnEnable()
    {

    }

    void OnDisable()
    {

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

    //between 2 concert
    private void SwitchOnOffSpotlight(bool value)
    {
        // TODO: switched off until wireup
        // canActivate = value;
    }
}
