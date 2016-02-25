using UnityEngine;
using System.Collections;


public class SpotlightUI : MonoBehaviour {

    public GameObject[] spotLights;

    [System.NonSerialized]
    public float aliveTime;

    private float passedTime;
    
	void Start () {
        DeactivateAll();
        passedTime = 0f;
    }
	
	void Update () {
	    if (passedTime <= 0)
        {
            DeactivateAll();
        } else
        {
            passedTime -= Time.deltaTime;
        }
	}

    public void DeactivateAll()
    {
        foreach(GameObject obj in spotLights) {
            obj.SetActive(false);
        }
    }
    
    public void Activate(GameObject musician)
    {
        foreach (GameObject obj in spotLights)
        {
            if (musician.name == obj.name)
            {
                obj.SetActive(true);
                passedTime = aliveTime;
            } else
            {
                obj.SetActive(false);
            }
        }
    }
}
