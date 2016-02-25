using UnityEngine;
using System.Collections;


public class SpotlightUI : MonoBehaviour {

    public GameObject[] spotLights;

    [System.NonSerialized]
    public float aliveTime;

    private float passedTime;
    private bool isActive;

    public GameObject particleEmitterPrefab;
    
	void Start () {
        DeactivateAll();
        passedTime = 0f;
        isActive = false;
    }
	
	void Update () {
        if (isActive)
        {
            if (passedTime <= 0)
            {
                isActive = false;
                DeactivateAll();
                GameObject inst = (GameObject)Instantiate(particleEmitterPrefab, Vector2.zero, Quaternion.identity);
                Destroy(inst, 3);
            }
            else
            {
                passedTime -= Time.deltaTime;
            }
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
                isActive = true;
                obj.SetActive(true);
                passedTime = aliveTime;
            } else
            {
                obj.SetActive(false);
            }
        }
    }
}
