using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RestartUI : MonoBehaviour
{
    public GameObject RestartPanel;

    #region Private fields
    RestartController restartController;
    #endregion

    public void SetController(RestartController controller)
    {
        restartController = controller;
    }

    public void OnRestartButtonClick()
    {
        HideUI();
        restartController.OnRestartGame();
    }

    public void OnBackToGameButtonClick()
    {
        HideUI();
        restartController.OnBackToGame();
    }

    public void HideUI()
    {
        RestartPanel.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        RestartPanel.gameObject.SetActive(true);
    }
}
