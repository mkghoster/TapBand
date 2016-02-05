using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

    private BallState state;

    private float countDown;
    private int previousSecond;

    private Rigidbody2D myRigidBody;

    public BallState State
    {
        set
        {
            state = value;
        }
    }

    void Start () {
        countDown = state.ballAliveTime;
        previousSecond = state.ballAliveTime;

        myRigidBody = GetComponent<Rigidbody2D>();
    }
	
	void Update () {
        HandleCountdown();
        HandleInput();
    }

    private void HandleCountdown()
    {
        float deltaTime = Time.deltaTime;

        countDown -= deltaTime;
        if (countDown <= 0)
        {
            // TODO : pass BallState
            Debug.Log(state.numberOfSuccessfulTaps);
            Destroy(gameObject);
        }
    }

    private void HandleInput()
    {
        if (Application.platform == RuntimePlatform.Android || 
            Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    CheckTouch(Input.GetTouch(0).position);
                }
            }
        } else {
            if (Input.GetMouseButtonDown(0))
            {
                CheckTouch(Input.mousePosition);
            }
        }
    }

    private void CheckTouch(Vector2 pos)
    {
        Vector2 wp = Camera.main.ScreenToWorldPoint(pos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        var hit = Physics2D.OverlapPoint(touchPos);

        if (hit)
        {
            myRigidBody.AddForce(new Vector2(Random.Range(-300, 300), 0));
            state.numberOfSuccessfulTaps++;
        }
    }
    
}
