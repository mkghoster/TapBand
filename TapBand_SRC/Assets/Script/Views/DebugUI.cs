using System.Collections;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public GameObject DebugPanel;

    #region Private fields
    DebugController debugController;
    #endregion

    public void SetController(DebugController controller)
    {
        debugController = controller;
    }

    public void BackToGameButtonClick()
    {
        HideUI();
        debugController.OnBackToGameClick();
    }

    public void AddCoinButtonClick()
    {
        debugController.AddCoins();
    }

    public void AddFanButtonClick()
    {
        debugController.AddFans();
    }

    public void AddTokenButtonClick()
    {
        debugController.AddTokens();
    }

    public void IncTapStrengthButtonClick()
    {
        debugController.IncTapStrength();
    }

    public void ResetDebugTapStrengthClick()
    {
        debugController.ResetDebugTapMultiplier();
    }

    public void HideUI()
    {
        DebugPanel.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        DebugPanel.gameObject.SetActive(true);
    }
}
