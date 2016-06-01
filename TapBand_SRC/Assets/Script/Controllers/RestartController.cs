using System.Collections;
using UnityEngine;

public delegate void NewLevelEvent();

public class RestartController : MonoBehaviour
{
    public event NewLevelEvent NewLevel;

    #region Private fields
    private RestartUI restartUI;
    private ViewController viewController;
    private BackstageController backstageController;
    #endregion

    void Awake()
    {
        restartUI = FindObjectOfType<RestartUI>();
        restartUI.SetController(this);

        viewController = FindObjectOfType<ViewController>();
        backstageController = FindObjectOfType<BackstageController>();

        backstageController.OnPrestigeButtonPressed += restartUI.ShowUI;
    }

    public void OnRestartGame()
    {
        if (NewLevel != null)
        {
            NewLevel();
        }
        viewController.EnterView(ViewType.STAGE);
    }

    public void OnBackToGame()
    {
        viewController.EnterView(ViewType.BACKSTAGE);
    }
}
