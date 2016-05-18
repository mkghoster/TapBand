using UnityEngine;
using System.Collections;

public class DroneControl : MonoBehaviour {
    float timer;

    Vector3 startPos;
    Vector3 startPos2;
    Vector3 zeroPos;
    Vector3 firstPos;

    void Start()
    {
        startPos = transform.position;
        MoveToStartPosition();
    }

    void MoveToStartPosition()
    {
        timer = Time.time;
        zeroPos = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (Time.time - timer > 1)
        {
            MoveToStartPosition();
        }
        transform.position = Vector3.Lerp(startPos, zeroPos, Time.time - timer);

    }
    void OnMouseDown()
    {
        this.gameObject.SetActive(false);
    }
    void Toucher()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log("Touch detected.");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast hit " + hit.collider.name);

                if (hit.collider.gameObject.tag == "new")
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

}
