using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoosterUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{ 

    public int tokenCost = 10;
    public bool boosterIsActive;
    public bool boosterIsAvailable;
    private BoosterDropZone boosterDropzone;
    private Vector3 basePosition;

    public void Awake()
    {
        boosterDropzone = GameObject.Find("BoosterDropZone").GetComponent<BoosterDropZone>();
        basePosition = GetComponent<RectTransform>().localPosition;
        if (boosterDropzone.tokenNumber > tokenCost)
        {
            boosterIsAvailable = true;
            boosterIsActive = false;
        }
        else
        {
            boosterIsAvailable = false;
        }


    }

    public void Start()
    {
        //Debug.Log(basePosition);
        //Debug.Log(this.gameObject.name);
    }

    public void Update()
    {
        //Debug.Log(boosterDropzone.tokenNumber);
        if (boosterDropzone.tokenNumber >= tokenCost && boosterIsAvailable)
        {
            boosterIsAvailable = true;
        }
        else
        {
            boosterIsAvailable = false;
            GetComponent<Button>().interactable = false;
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Color color = new Vector4(0.5f, 0.5f, 0.5f, 0.6f);
        boosterDropzone.setColor(color);
        if (boosterIsAvailable && !boosterIsActive)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (boosterIsAvailable && !boosterIsActive)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Color color = new Vector4(0f, 0f, 0f, 0f);
        boosterDropzone.setColor(color);
        if (!boosterIsAvailable)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }   
        transform.localPosition = basePosition;
    }

    public bool IsActive()
    {
        return boosterIsActive;
    }

    public bool IsAvailable()
    {
        return boosterIsAvailable;
    }
}
