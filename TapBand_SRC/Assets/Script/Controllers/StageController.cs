using System;
using System.Collections;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public event Action OnEncoreButtonPressed;

    #region Private fields
    private StageUI stageUI;
    private SongController songController;
    private ConcertController concertController;
    private ViewController viewController;
    private DailyEventController dailyEventController;
    #endregion

    void Awake()
    {
        stageUI = FindObjectOfType<StageUI>();
        stageUI.SetController(this);

        songController = FindObjectOfType<SongController>();
        concertController = FindObjectOfType<ConcertController>();
        viewController = FindObjectOfType<ViewController>();
        dailyEventController = FindObjectOfType<DailyEventController>();
    }

    void OnEnable()
    {
        songController.OnSongStarted += HandleSongStarted;
        songController.OnSongFinished += HandleSongFinished;

        concertController.ShowEncoreButton += ShowEncoreButton;
        concertController.HideEncoreButton += HideEncoreButton;

        viewController.OnViewChange += ViewChanged;

        dailyEventController.OnDailyEventStarted += HandleDailyEventStarted;
    }

    void OnDisable()
    {
        songController.OnSongStarted -= HandleSongStarted;
        songController.OnSongFinished -= HandleSongFinished;

        concertController.ShowEncoreButton -= ShowEncoreButton;
        concertController.HideEncoreButton -= HideEncoreButton;

        viewController.OnViewChange -= ViewChanged;

        dailyEventController.OnDailyEventStarted -= HandleDailyEventStarted;
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


    private void ShowEncoreButton()
    {
        stageUI.ActivateEncoreButton();
    }

    private void HideEncoreButton()
    {
        stageUI.DeactivateEncoreButton();
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

    private void HandleDailyEventStarted(object sender, EventArgs e)
    {
        stageUI.HideUI();
    }
}
