using UnityEngine;
using System.Collections;

public class DroneHandler : MonoBehaviour {
    public GameObject Drone;
    public GameObject DroneMover;
    MoveToNextStation ColliderScript;
    public BoxCollider boxcolcaller;
    public int stationChooser;
    public int i;
    public int j = 5;
    public int k = 0;
    public bool starter=true;

    IEnumerator DroneCycle()
    {
        while (i<=5)
        {
            if (i % 2 == 0)
            {
                stationChooser = Random.Range(0, 2);
            }
            else
            {
                stationChooser = Random.Range(3, 5);
            }
            DroneMover.SetActive(true);
            yield return new WaitForSeconds(3);
            i++;
            DroneMover.SetActive(false);
            if (i == 5)
            {
                starter = true;
                Drone.SetActive(false);
                j = 5;
            }
        }

    }

    void Start()
    {
        ColliderScript = DroneMover.GetComponent<MoveToNextStation>();
        boxcolcaller = ColliderScript.boxcol;
    }

    void Update ()
    {
        boxcolcaller.enabled = true;
        if (starter==true)
        {
            StartCoroutine(DroneCycle());
            starter = false;
            i = 0;
        }else if (j <= 0)
        {
            Drone.SetActive(false);
            starter = true;
            j = 5;
            i = 0;
        }else if (Input.GetMouseButtonDown(0))
            {
                Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rhInfo;
                bool didHit = Physics.Raycast(toMouse, out rhInfo, Mathf.Infinity);
                if (didHit)
                {
                    if (i % 2 == 0)
                    {
                        stationChooser = Random.Range(3, 5);
                    }
                    else
                    {
                        stationChooser = Random.Range(0, 2);
                    }
                    DroneMover.SetActive(true);
                    //Debug.Log(i);
                    j--;
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
