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

        //boosterDropzone = GameObject.Find("BoosterDropZone").GetComponent<BoosterDropZone>();
        //basePosition = GetComponent<RectTransform>().localPosition;
        basePosition = GetComponent<RectTransform>().anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
        //if (boosterDropzone.tokenNumber > tokenCost)
        //{
        //    boosterIsAvailable = true;
        //    boosterIsActive = false;
        //}
        //else
        //{
        //    boosterIsAvailable = false;
        //}
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
        //if (boosterIsAvailable && !boosterIsActive)
        //{
        this.transform.position = eventData.position;
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //boosterDropzone.setColor(color);
        //if (!boosterIsAvailable)
        //{
        //    GetComponent<CanvasGroup>().blocksRaycasts = false;
        //}
        //else
        //{
        //    GetComponent<CanvasGroup>().blocksRaycasts = true;
        //}
        //transform.localPosition = basePosition;
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
