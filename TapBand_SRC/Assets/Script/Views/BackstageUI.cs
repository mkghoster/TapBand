using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackstageUI : MonoBehaviour
{
    public GameObject BackstagePanel;

    #region Private fields
    BackstageController backstageController;
    #endregion

    public void SetController(BackstageController controller)
    {
        backstageController = controller;
    }

    public void OnBackToStageButtonClick()
    {
        HideUI();
        backstageController.SwitchToStage();
    }

    public void OnMerchButtonClick()
    {
        HideUI();
        backstageController.OnMerchClick();
    }

    public void OnPrestigeButtonClick()
    {
        HideUI();
        backstageController.OnPrestigeClick();
    }

    public void OnDressingRoomButtonClick()
    {
        HideUI();
        backstageController.SwitchDressingRoom(true);
    }

    public void OnSettingsButtonClick()
    {
        HideUI();
        backstageController.OnSettingsClick();
    }

    public void HideUI()
    {
        BackstagePanel.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        BackstagePanel.gameObject.SetActive(true);
    }

    public void OnDebugButtonClick()
    {
        HideUI();
        backstageController.OnDebugClick();
    }
}
