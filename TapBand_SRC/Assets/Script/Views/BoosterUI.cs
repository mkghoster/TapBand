using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BoosterUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{ 

    public int tokenCost = 10;
    public bool boosterIsActive;
    private BoosterDropZone boosterDropzone;
    public Vector3 basePosition;

    public void Awake()
    {
        boosterDropzone = GameObject.Find("BoosterDropZone").GetComponent<BoosterDropZone>();
        basePosition = GetComponent<RectTransform>().localPosition;
        if (boosterDropzone.tokenNumber > tokenCost)
        {
            boosterIsActive = true;
        }
        else
        {
            boosterIsActive = false;
        }


    }

    public void Start()
    {
        Debug.Log(basePosition);
        Debug.Log(this.gameObject.name);
    }

    public void Update()
    {
        if (boosterDropzone.tokenNumber >= tokenCost)
        {
            //boosterIsActive = true;
            //Debug.Log(boosterIsActive);
        }
        else
        {
            boosterIsActive = false;
        }


    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Color color = new Vector4(0.5f, 0.5f, 0.5f, 0.6f);
        boosterDropzone.setColor(color);
        if (boosterIsActive)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (boosterIsActive)
        {
            this.transform.position = eventData.position;

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Color color = new Vector4(0f, 0f, 0f, 0f);
        boosterDropzone.setColor(color);
        if (!boosterIsActive)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public bool IsActive()
    {
        return boosterIsActive;
    }
}
