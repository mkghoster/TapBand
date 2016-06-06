using System;
using System.Collections;
using UnityEngine;

public class BackstageController : MonoBehaviour
{
    public event Action OnMerchButtonPressed;
    public event Action OnPrestigeButtonPressed;
    public event Action OnDebugButtonPressed;

    #region Private fields
    private BackstageUI backstageUI;
    private DressingRoomUI dressingRoomUI;
    private ViewController viewController;

    private CharacterType currentBackstageCharacter = CharacterType.Bass;
    #endregion

    void Awake()
    {
        backstageUI = FindObjectOfType<BackstageUI>();

        dressingRoomUI = FindObjectOfType<DressingRoomUI>();

        viewController = FindObjectOfType<ViewController>();

        viewController.OnViewChange += ViewChanged;
    }

    void Start()
    {
        backstageUI.SetController(this);
        dressingRoomUI.SetController(this);
    }

    private void ViewChanged(object sender, ViewChangeEventArgs e)
    {
        if (e.NewView == ViewType.BACKSTAGE)
        {
            backstageUI.ShowUI();
        }
    }

    public void SwitchToStage()
    {
        viewController.EnterView(ViewType.STAGE);
    }

    public void OnMerchClick()
    {
        if (OnMerchButtonPressed != null)
        {
            OnMerchButtonPressed();
        }
    }

    public void OnPrestigeClick()
    {
        if (OnPrestigeButtonPressed != null)
        {
            OnPrestigeButtonPressed();
        }
    }

    public void SwitchDressingRoom(bool dressingRoom)
    {
        if (dressingRoom)
        {
            viewController.EnterView(ViewType.CUSTOMIZATION); // TODO: animate camera
            dressingRoomUI.ShowUI();
        }
        else
        {
            viewController.EnterView(ViewType.BACKSTAGE);
            backstageUI.ShowUI();
        }
    }

    public void OnDebugClick()
    {
        if (OnDebugButtonPressed != null)
        {
            OnDebugButtonPressed();
        }
    }
}
