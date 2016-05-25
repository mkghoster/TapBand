using UnityEngine;
using System.Collections;

public class LerpPractice : MonoBehaviour {
    public GameObject Drone;
    private Vector3 startPos;
    private Vector3 endPos;
    public float LerpTime = 2f;
    private float currentLerpTime = 0f;
    //public static bool DroneTouch=false;// = false;
    private bool DroneTouch = false;
    //public static int k=0;


    Vector3[] rightStations = {
	    new Vector3 (-1.6f, -3, 0),
	    new Vector3 (-1.6f,0, 0),
	    new Vector3 (-1.6f, 3f, 0)
	};
    Vector3[] leftStations = {
        new Vector3 (1.6f, -3, 0),
        new Vector3 (1.6f,0, 0),
        new Vector3 (1.6f, 3, 0),
        new Vector3 (1.6f,0, 0),
        new Vector3 (1.6f, 3, 0)
    };

    // Use this for initialization
    void Start () {
        startPos = Drone.transform.position;
        int i = Random.Range(0, 2);
        endPos = leftStations[3];
	}
	
	// Update is called once per frame
	void Update () {


            if (DroneTouch == true)
            {
                currentLerpTime += Time.deltaTime;
                if (currentLerpTime >= LerpTime)
                {
                    currentLerpTime = LerpTime;
                }
                float Perc = currentLerpTime / LerpTime;
                Drone.transform.position = Vector3.Lerp(startPos, endPos, Perc);
            }
    }
    void OnMouseDown()
    {
       DroneTouch = true;
    }

}
