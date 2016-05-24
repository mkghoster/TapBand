using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform backstageTransform;
    public Transform dressingRoomTransform; // TODO: ezeket kikeresni / hardcodeolni / prefabba rakni

    public void OnBackstageClick()
    {
        transform.position = new Vector3(backstageTransform.position.x, backstageTransform.position.y, transform.position.z);
    }

    public void OnDressingRoomClick()
    {
        transform.position = new Vector3(dressingRoomTransform.position.x, dressingRoomTransform.position.y, transform.position.z);
    }

    public void OnBackToStageClick()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
    }
}
