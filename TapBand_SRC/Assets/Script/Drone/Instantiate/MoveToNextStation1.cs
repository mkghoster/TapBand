using UnityEngine;
using System.Collections;

public class MoveToNextStation1 : MonoBehaviour {
    public GameObject Drone;
    public GameObject DroneMover;
    public GameObject BoxCollider;
    public Transform Stations;
    public Vector3 endpos;
    public Vector3 startpos;//=new Vector3(0,0,0);
    public BoxCollider boxcol;
    public float LerpTime=20.0f;// = 10f;
    private float currentLerpTime = 0.0f;
    DroneHandler1 StationChoserScript;
    public Animator animator;

    const int STATE_IDLE = 0;
    const int STATE_RIGHT = -1;
    const int STATE_LEFT = 1;
    int _currentAnimationState = STATE_IDLE;
    //-----------------------------------------------------------------------------------------------------------
    void Start () {
    }
    //-----------------------------------------------------------------------------------------------------------
	void Update () {

        StationChoserScript = Drone.GetComponent<DroneHandler1>();
        int numStations = Stations.childCount;
        Vector3[] stations = new Vector3[numStations];
        for (int i = 0; i < numStations; ++i)
        {
            stations[i] = Stations.GetChild(i).transform.position;
        }

        startpos = Drone.transform.position;
        endpos = stations[StationChoserScript.stationChooser];
        if (startpos.x > endpos.x)
        {
            changeState(STATE_LEFT);
            //animator.SetInteger("DroneAnimState", 1);
        }
        else
        {
            changeState(STATE_RIGHT);
            //animator.SetInteger("DroneAnimState", -1);
        }
        LerpTime = 20.0f;
        currentLerpTime += Time.deltaTime;
        BoxCollider.SetActive(false);
        if (currentLerpTime >= LerpTime-19.0f)
        {
            //changeState(STATE_IDLE);
            BoxCollider.SetActive(true);
            StartCoroutine(Levitating());
            currentLerpTime = 0;

        }
        float Perc = currentLerpTime / LerpTime;
        Drone.transform.position = Vector3.Lerp(startpos, endpos, Perc);
    }
    //-------------------------------------------------------------------------------------------------------------------

    IEnumerator Levitating()
    {
        DroneMover.SetActive(false);
        //animator.SetInteger("DroneAnimState", 0);
        yield return new WaitForSeconds(1);
    }
    void changeState(int state)
    {
        if (_currentAnimationState == state)
            return;
        switch (state)
        {
            case STATE_IDLE:
                animator.SetInteger("DroneAnimState", STATE_IDLE);
                break;

            case STATE_RIGHT:
                animator.SetInteger("DroneAnimState", STATE_RIGHT);
                break;

            case STATE_LEFT:
                animator.SetInteger("DroneAnimState", STATE_LEFT);
                break;
        }

        _currentAnimationState = state;
    }
}
