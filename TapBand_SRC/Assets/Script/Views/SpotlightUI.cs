using UnityEngine;
using System.Collections;
using QuickPool;

public class SpotlightUI : MonoBehaviour {

    [System.NonSerialized]
    public float aliveTime;

    private GameObject[] spotlights;
    private float passedTime;
    private bool isActive;
    private Pool spotlightEmitterPool;
    private GameObject lastEmitter;
   
	void Start () {
        spotlightEmitterPool = PoolsManager.Instance["SpotlightParticleEmitter"];
        lastEmitter = null;
        spotlights = GameObject.FindGameObjectsWithTag(Tags.SPOTLIGHT);
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

                if (lastEmitter != null)
                {
                    spotlightEmitterPool.Despawn(lastEmitter);
                }
                lastEmitter = spotlightEmitterPool.Spawn(Vector3.zero, Quaternion.identity);
            }
            else
            {
                passedTime -= Time.deltaTime;
            }
        }
	}

    public void DeactivateAll()
    {
        foreach(GameObject obj in spotlights) {
            obj.SetActive(false);
        }
    }
    
    public void Activate(GameObject musician)
    {
        foreach (GameObject obj in spotlights)
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
