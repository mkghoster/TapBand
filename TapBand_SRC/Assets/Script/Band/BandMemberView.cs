using UnityEngine;
using System.Collections;

public abstract class BandMemberView : MonoBehaviour {

    private float shakeY;
    private float shakeYSpeed = 0.8f;

    protected abstract void TapEventTrigger();

    void OnMouseDown()
    {
        TapEventTrigger();
        if (shakeY == 0.0)
            shakeY = 0.5f;
    }
    
    void Update()
    {
        Animate();
    }

    // TODO: Only for tests, remove it later!
    public void ForceClickOnBandMember()
    {
        this.shakeYSpeed = Random.Range(0.3f, 0.9f);
        OnMouseDown();
    }

    private void Animate()
    {
        Vector3 newPosition = new Vector3(0, shakeY, 0);
        if (shakeY < 0)
        {
            shakeY *= shakeYSpeed;
        }
        shakeY = -shakeY;
        transform.Translate(newPosition, Space.Self);

        if (shakeY < 0.2 && shakeY > -0.2)
        {
            shakeY = 0.0f;
        }
    }

}
