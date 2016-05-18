using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BoosterUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{ 

    public int tokenCost = 10;
    public bool boosterIsActive;
    public bool boosterIsAvailable;
    private BoosterDropZone boosterDropzone;
    public Vector3 basePosition;

    public void Awake()
    {
        boosterDropzone = GameObject.Find("BoosterDropZone").GetComponent<BoosterDropZone>();
        basePosition = GetComponent<RectTransform>().localPosition;
        if (boosterDropzone.tokenNumber > tokenCost)
        {
            //boosterIsActive = true;
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
        Debug.Log(basePosition);
        Debug.Log(this.gameObject.name);
    }

    public void Update()
    {
        if (boosterDropzone.tokenNumber >= tokenCost && boosterIsAvailable)
        {
            //boosterIsActive = true;
            boosterIsAvailable = true;
            //Debug.Log(boosterIsActive);
        }
        else
        {
            boosterIsAvailable = false;
            //boosterIsActive = false;
        }

       // Debug.Log(boosterIsActive);

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("boosteractice:"+boosterIsActive);
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
       // Debug.Log("boosteractice end:" + boosterIsAvailable);
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
