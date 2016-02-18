using UnityEngine;
using System.Collections;
using System;

public class SpotlightController : MonoBehaviour
{

    private float SpotlightInterval;
    private float SpotlightMinDelay;
    private float SpotlightMaxDelay;

    private float initSpotlightCountdown;
    private float actualCountDown;

    public SpotlightUI spotlightUI;

    public GameObject[] musicians;

    void Start()
    {
        SpotlightInterval = (float)Convert.ToDouble(GameData.instance.FindGeneralDataByName(GeneralData.SpotlightInterval).value);
        SpotlightMinDelay = (float)Convert.ToDouble(GameData.instance.FindGeneralDataByName(GeneralData.SpotlightMinDelay).value);
        SpotlightMaxDelay = (float)Convert.ToDouble(GameData.instance.FindGeneralDataByName(GeneralData.SpotlightMaxDelay).value);

        initSpotlightCountdown = SpotlightInterval;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (initSpotlightCountdown <= 0)
        {
            int indexToActivate = UnityEngine.Random.Range(0, 4);
            spotlightUI.Activate(musicians[indexToActivate], UnityEngine.Random.Range(SpotlightMinDelay, SpotlightMaxDelay));
            initSpotlightCountdown = SpotlightInterval;
        } else
        {
            initSpotlightCountdown -= dt;
        }


    }
}
