using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

    private BallState state;

    private float countDown;
    private Rigidbody2D myRigidBody;
    private GameObject particleEmitter;

    private bool left = false;

    public BallState State
    {
        set
        {
            state = value;
        }
    }

    void Start () {
        countDown = state.ballAliveTime;
        myRigidBody = GetComponent<Rigidbody2D>();
        particleEmitter = GameObject.Find("BallEmitter");
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
        Collider2D hit = Physics2D.OverlapPoint(touchPos);

        if (hit)
        {
            KickBall();
            InitializeEmitter();
            state.numberOfSuccessfulTaps++;
        }
    }
    
    private void InitializeEmitter()
    {
        GameObject newEmitter = Instantiate(particleEmitter);
        newEmitter.transform.position = gameObject.transform.position;

        ParticleSystem ps = newEmitter.GetComponent<ParticleSystem>();
        Destroy(newEmitter, ps.duration + ps.startLifetime);
    }

    private void KickBall()
    {
        int verticalForce = (left ? -1 : 1) * Random.Range(150, 250);
        left = !left;
        myRigidBody.AddForce(new Vector2(verticalForce, 0));
    }
}
