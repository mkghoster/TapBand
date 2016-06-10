using UnityEngine;
using System.Collections;

public class DroneHandler1 : MonoBehaviour {
    public GameObject Drone;
    public GameObject DroneMover;
    //MoveToNextStation ColliderScript;
    //public BoxCollider boxcolcaller;
    public int stationChooser;
    public int NumberOfReachedStations;
    public int DroneLife = 5;
    public bool starter=true;

    IEnumerator DroneCycle()
    {
        while (NumberOfReachedStations<=5)
        {
            if (NumberOfReachedStations % 2 == 0)
            {
                stationChooser = Random.Range(0, 2);
            }
            else
            {
                stationChooser = Random.Range(3, 5);
            }
            DroneMover.SetActive(true);
            yield return new WaitForSeconds(4);
            NumberOfReachedStations++;
            if (NumberOfReachedStations == 5)
            {
                stationChooser = 6;
                DroneMover.SetActive(true);
                yield return new WaitForSeconds(4);
                Drone.SetActive(false);
                starter = true;
                DroneLife = 5;
            }
        }

    }

    void Start()
    {
        //starter = true;
        //ColliderScript = DroneMover.GetComponent<MoveToNextStation>();
        //boxcolcaller = ColliderScript.boxcol;
    }

    void Update ()
    {
        if (starter==true)
        {
            NumberOfReachedStations = 0;
            StartCoroutine(DroneCycle());
            starter = false;
        }else if (DroneLife <= 0)
        {
            DroneMover.SetActive(false);
            Drone.SetActive(false);
            //Destroy(Drone, 0);
            starter = true;
            DroneLife = 5;
            NumberOfReachedStations = 0;
        }else if (Input.GetMouseButtonDown(0))
            {
                Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rhInfo;
                bool didHit = Physics.Raycast(toMouse, out rhInfo, Mathf.Infinity);
                if (didHit)
                {
                    if (NumberOfReachedStations % 2 == 0)
                    {
                        stationChooser = Random.Range(3, 5);
                    }
                    else
                    {
                        stationChooser = Random.Range(0, 2);
                    }
                    DroneMover.SetActive(true);
                    DroneLife--;
                }
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray;
                RaycastHit hit;
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                bool didHit = Physics.Raycast(ray, out hit, Mathf.Infinity);
                if (didHit)
                {
                }

            }      
	}

}
