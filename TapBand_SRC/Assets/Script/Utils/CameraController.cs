using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform backstageTransform;
    public Transform dressingRoomTransform; // TODO: ezeket kikeresni / hardcodeolni / prefabba rakni

    #region Private fields
    private ViewController viewController;
    #endregion

    public void Awake()
    {
        viewController = FindObjectOfType<ViewController>();
        viewController.OnViewChange += ViewChanged;
    }

    private void ViewChanged(object sender, ViewChangeEventArgs e)
    {
        switch (e.NewView)
        {
            case ViewType.BACKSTAGE:
                FocusToBackstage();
                return;

            case ViewType.CUSTOMIZATION:
                FocusToDressingRoom();
                return;

            case ViewType.STAGE:
                FocusToStage();
                return;

        }
    }

    public void FocusToBackstage()
    {
        transform.position = new Vector3(backstageTransform.position.x, backstageTransform.position.y, transform.position.z);
    }

    public void FocusToDressingRoom()
    {
        transform.position = new Vector3(dressingRoomTransform.position.x, dressingRoomTransform.position.y, transform.position.z);
    }

    public void FocusToStage()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
    }
}
