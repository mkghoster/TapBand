using System;
using System.Collections;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public event Action OnEncoreButtonPressed;
    public event Action OnDebugButtonPressed;

    #region Private fields
    private StageUI stageUI;
    private SongController songController;
    private ConcertController concertController;
    private ViewController viewController;
    #endregion

    void Awake()
    {
        stageUI = FindObjectOfType<StageUI>();
        stageUI.SetController(this);

        songController = FindObjectOfType<SongController>();
        concertController = FindObjectOfType<ConcertController>();
        viewController = FindObjectOfType<ViewController>();

        songController.OnSongStarted += HandleSongStarted;
        songController.OnSongFinished += HandleSongFinished;

        viewController.OnViewChange += ViewChanged;
    }

    private void HandleSongStarted(object sender, SongEventArgs e)
    {
        if (concertController.HasTriedEncore && concertController.IsNextSongEncore)
        {
            stageUI.ActivateEncoreButton();
        }
    }

    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        stageUI.DeactivateEncoreButton();
    }

    public void OnEncoreClick()
    {
        if (OnEncoreButtonPressed != null)
        {
            OnEncoreButtonPressed();
        }
    }

    public void OnDebugClick()
    {
        if (OnDebugButtonPressed != null)
        {
            OnDebugButtonPressed();
        }
    }

    public void SwitchToBackstage()
    {
        viewController.EnterView(ViewType.BACKSTAGE);
    }

    private void ViewChanged(object sender, ViewChangeEventArgs e)
    {
        if (e.NewView == ViewType.STAGE)
        {
            stageUI.ShowUI();
        }
    }
}
