using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform backstageTransform;
    public Transform dressingRoomTransform;

    #region Private fields
    private ViewController viewController;
    private Transform mainCamera;
    #endregion

    public void Awake()
    {
        viewController = FindObjectOfType<ViewController>();
        mainCamera = transform.FindChild("MainCamera");
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
        mainCamera.position = new Vector3(backstageTransform.position.x, backstageTransform.position.y, mainCamera.position.z);
    }

    public void FocusToDressingRoom()
    {
        mainCamera.position = new Vector3(dressingRoomTransform.position.x, dressingRoomTransform.position.y, mainCamera.position.z);
    }

    public void FocusToStage()
    {
        mainCamera.position = new Vector3(0, 0, mainCamera.position.z);
    }
}
