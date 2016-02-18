using UnityEngine;
using System.Collections;


public class SpotlightUI : MonoBehaviour {

    public GameObject[] spotLights;

    private float initAliveTime;
    
	void Start () {
        DeactivateAll();
        initAliveTime = 0f;
    }
	
	void Update () {
	    if (initAliveTime <= 0)
        {
            DeactivateAll();
        } else
        {
            initAliveTime -= Time.deltaTime;
        }
	}

    public void DeactivateAll()
    {
        foreach(GameObject obj in spotLights) {
            obj.SetActive(false);
        }
    }
    
    public void Activate(GameObject musician, float aliveTime)
    {
        foreach (GameObject obj in spotLights)
        {
            if (musician.name == obj.name)
            {
                obj.SetActive(true);
                initAliveTime = aliveTime;
            } else
            {
                obj.SetActive(false);
            }
        }
    }
}
