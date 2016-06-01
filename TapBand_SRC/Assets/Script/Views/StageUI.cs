using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    public Button EncoreButton;
    //public Button DebugButton;
    public GameObject StagePanel;

    #region Private fields
    StageController stageController;
    #endregion

    public void SetController(StageController controller)
    {
        stageController = controller;
    }

    public void ActivateEncoreButton()
    {
        EncoreButton.gameObject.SetActive(true);
    }

    public void DeactivateEncoreButton()
    {
        EncoreButton.gameObject.SetActive(false);
    }

    public void OnEncoreButtonClick()
    {
        stageController.OnEncoreClick();
        DeactivateEncoreButton();
    }

    public void OnBackstageButtonClick()
    {
        HideUI();
        stageController.SwitchToBackstage();
    }

    public void OnDebugButtonClick()
    {
        HideUI();
        stageController.OnDebugClick();
    }

    public void HideUI()
    {
        StagePanel.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        StagePanel.gameObject.SetActive(true);
    }
}
