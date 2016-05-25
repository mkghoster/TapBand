using UnityEngine;
using System.Collections;

public class MoveToNextStation : MonoBehaviour {
    public GameObject Drone;
    public GameObject DroneMover;
    public Transform Stations;
    public Vector3 endpos;
    public Vector3 startpos=new Vector3(0,0,0);
    public BoxCollider boxcol;
    public float LerpTime;// = 10f;
    private float currentLerpTime = 0.0f;
    DroneHandler StationChoserScript;

    public float speed = 1.0f;
    public float radius = 1.0f;
    Vector3 center = Vector3.zero;

    void Start () {

    }

	void Update () {

        StationChoserScript = Drone.GetComponent<DroneHandler>();
        int numStations = Stations.childCount;
        Vector3[] stations = new Vector3[numStations];
        for (int i = 0; i < numStations; ++i)
        {
            stations[i] = Stations.GetChild(i).transform.position;
        }

        startpos = Drone.transform.position;
        endpos = stations[StationChoserScript.stationChooser];
        LerpTime = 0.5f;
        currentLerpTime += Time.deltaTime;
        boxcol.enabled = false;
        if (currentLerpTime >= LerpTime)
        {
            StartCoroutine(Levitating());
            //DroneMover.SetActive(false);
            //Debug.Log("zagyva");
            currentLerpTime = 0;

        }
        float Perc = currentLerpTime / LerpTime;
        Drone.transform.position = Vector3.Lerp(startpos, endpos, Perc);
    }


    IEnumerator Levitating()
    {
        Debug.Log("körözök");
        center =startpos;
        float t = Time.time;
        Vector3 pos = center;
        pos.x += radius * Mathf.Cos(t * speed);
        pos.y += radius * Mathf.Sin(t * speed);
        transform.position = pos;
        DroneMover.SetActive(false);
        yield return new WaitForSeconds(1);
    }
}
