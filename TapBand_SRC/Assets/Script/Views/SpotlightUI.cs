using UnityEngine;
using System.Collections;
using QuickPool;

public class SpotlightUI : MonoBehaviour
{
    private GameObject[] spotlights;
    private int lastSpotlight;

    void Start()
    {
        spotlights = GameObject.FindGameObjectsWithTag(Tags.SPOTLIGHT);
        lastSpotlight = -1;
        DeactivateAll();
    }

    public void DeactivateAll()
    {
        foreach (GameObject obj in spotlights)
        {
            obj.SetActive(false);
        }
        lastSpotlight = -1;
    }

    public void ChangeSpotlight()
    {
        var nextSpotlight = lastSpotlight;
        while (nextSpotlight == lastSpotlight)
        {
            nextSpotlight = Random.Range(0, spotlights.Length);
        }
        if (lastSpotlight >= 0)
        {
            spotlights[lastSpotlight].SetActive(false);
        }
        spotlights[nextSpotlight].SetActive(true);
        lastSpotlight = nextSpotlight;
    }
}
