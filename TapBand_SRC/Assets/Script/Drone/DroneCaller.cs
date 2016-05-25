using UnityEngine;
using System.Collections;

public class DroneCaller : MonoBehaviour {

    public GameObject Drone;
    public int DroneMaxInterval = 5; //max alivetime
    int DroneMinDelay;
    int DroneMaxDelay;
    int deltaTime;
    DroneHandler StarterScript;
    public bool starterr;
    public int ihandler;

    void Start () {
        DroneMinDelay = 10;
        DroneMaxDelay = 13;
        deltaTime = Random.Range(DroneMinDelay, DroneMaxDelay);
        StartCoroutine(waitSomeSeconds());
    }

    IEnumerator waitSomeSeconds()
    {
        while(true)
        {
            StarterScript = Drone.GetComponent<DroneHandler>();
            starterr = StarterScript.starter;
            yield return new WaitForSeconds(deltaTime);
            Drone.transform.position = new Vector3(0, -0, 0);
            //ihandler = StarterScript.i;
            //ihandler = 0;
            starterr = true;
            Drone.SetActive(true);
            //starterr = false;
        }

    }
	void Update () {
	
	}
}
