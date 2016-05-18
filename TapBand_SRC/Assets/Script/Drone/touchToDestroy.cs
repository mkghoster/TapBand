using UnityEngine;
using System.Collections;

public class touchToDestroy : MonoBehaviour {
    public GameObject Drone;
    void OnMouseDown()
    {
        //Destroy(gameObject);
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
                    Debug.Log("The tag matches.");
                    //Destroy(gameObject);
                    Drone.SetActive(false);

                }
            }
        }
    }

    /*
    private void HandleInput()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began) ;
                {
                    CheckTouch(Input.GetTouch(0).position);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    CheckTouch(Input.mousePosition);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
    private void CheckTouch(Vector2 pos)
    {
        Vector2 wp = Camera.main.ScreenToWorldPoint(pos);
        Vector2 touchpos = new Vector2(wp.x, wp.y);
        Collider2D hit = Physics2D.OverlapPoint(touchpos);
        if (hit)
        {
            state.numberOfSuccessfulTaps++;
        }
    }*/

}
