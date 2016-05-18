using UnityEngine;
using System.Collections;

public class DroneCaller : MonoBehaviour {

    public GameObject Drone;
    public int DroneMaxInterval = 10;
    public int DroneMinDelay = 10;
    public int DroneMaxDelay = 30;
    int deltaTime;
    //int deltaTime = Random.Range(DroneMinDelay, DroneMaxDelay);

    // Use this for initialization
    void Start () {
        //deltaTime = Random.Range(2, 4);
        StartCoroutine(waitSomeSeconds());

    }
	

    IEnumerator waitSomeSeconds()
    {
        while(true)
        {
            deltaTime = Random.Range(2, 4);
            yield return new WaitForSeconds(deltaTime);
            Drone.transform.position = new Vector3(10, 6, 0);
            Drone.SetActive(true);
            //Drone.transform.position = new Vector3(10, 6, 0);
            //Drone.SetActive(true);

        }

    }
	void Update () {
	
	}
}
