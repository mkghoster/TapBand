using UnityEngine;
using System.Collections;

public class BallInitializerController : MonoBehaviour {

    public GameObject prefabToInstantiate;

	void Start () {
        GameObject aliveBall = Instantiate<GameObject>(prefabToInstantiate);
        aliveBall.transform.position = Vector2.zero;

        BallController controller = aliveBall.GetComponent<BallController>();
        controller.State = new BallState(20);
    }

    void Update () {
	
	}
}
