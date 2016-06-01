using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class BoosterItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BoosterType boosterType;

    private BoosterController boosterController;
    private SongController songController;

    private Vector3 basePosition;

    public event BoosterEvent OnBeginBoosterDrag;
    public event BoosterEvent OnEndBoosterDrag;

    private CanvasGroup canvasGroup;

    public void Awake()
    {
        boosterController = FindObjectOfType<BoosterController>();
        songController = FindObjectOfType<SongController>();

        basePosition = GetComponent<RectTransform>().anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnEnable()
    {
        boosterController.OnBoosterStateChanged += HandleActivationRelevantEvents;
        boosterController.OnBoosterActivated += HandleActivationRelevantEvents;
        boosterController.OnBoosterFinished += HandleActivationRelevantEvents;
        HandleActivationRelevantEvents(null, null);
    }

    private void OnDisable()
    {
        boosterController.OnBoosterStateChanged -= HandleActivationRelevantEvents;
        boosterController.OnBoosterActivated -= HandleActivationRelevantEvents;
        boosterController.OnBoosterFinished -= HandleActivationRelevantEvents;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //boosterDropzone.setColor(color);
        if (boosterController.CanActivateBooster(boosterType))
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            if (OnBeginBoosterDrag != null)
            {
                OnBeginBoosterDrag(this, new BoosterEventArgs(boosterType));
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnEndBoosterDrag != null)
        {
            OnEndBoosterDrag(this, new BoosterEventArgs(boosterType));
        }
        gameObject.GetComponent<RectTransform>().anchoredPosition = basePosition;
        canvasGroup.blocksRaycasts = true;
    }

    private void HandleActivationRelevantEvents(object sender, EventArgs e)
    {
        canvasGroup.blocksRaycasts = boosterController.CanActivateBooster(boosterType);
    }
}
