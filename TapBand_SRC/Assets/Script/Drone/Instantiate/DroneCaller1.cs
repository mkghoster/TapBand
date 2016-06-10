using UnityEngine;
using System.Collections;

public class DroneCaller1 : MonoBehaviour {

    public GameObject Drone;
    //public GameObject DroneMover;
    //public int DroneMaxInterval = 5; //max alivetime
    int DroneMinDelay;
    int DroneMaxDelay;
    int deltaTime;
    Vector3 alphapos = new Vector3(5, 3, 0);
    //DroneHandler StarterScript;
    //public bool starterr;
    //public int ihandler;

    void Start () {
        DroneMinDelay = 30;
        DroneMaxDelay = 31;
        deltaTime = Random.Range(DroneMinDelay, DroneMaxDelay);
        StartCoroutine(waitSomeSeconds());
    }

    IEnumerator waitSomeSeconds()
    {
        while(true)
        {
            //StarterScript = Drone.GetComponent<DroneHandler>();
            //starterr = StarterScript.starter;
            //starterr = true;
            yield return new WaitForSeconds(deltaTime);
            Drone.SetActive(true);
        }

    }
	void Update () {
	
	}
}
