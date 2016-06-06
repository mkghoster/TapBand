using System;
using System.Collections;
using UnityEngine;

public class BackstageController : MonoBehaviour
{
    public event Action OnMerchButtonPressed;
    public event Action OnPrestigeButtonPressed;

    #region Private fields
    private BackstageUI backstageUI;
    private ViewController viewController;
    #endregion

    void Awake()
    {
        backstageUI = FindObjectOfType<BackstageUI>();
        backstageUI.SetController(this);

        viewController = FindObjectOfType<ViewController>();

        viewController.OnViewChange += ViewChanged;
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
}
