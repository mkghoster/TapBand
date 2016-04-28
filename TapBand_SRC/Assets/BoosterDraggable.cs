using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BoosterDraggable : MonoBehaviour {

    //	public int tokenCost; IBeginDragHandler, IDragHandler, IEndDragHandler
    //	private bool boosterIsActive;
    //	private BoosterDropZone boosterDropzone;
    //	public Vector3 basePosition;

    //	public void Awake(){
    //		boosterDropzone = GameObject.Find ("BoosterDropZone").GetComponent<BoosterDropZone>();
    //        basePosition = GetComponent<RectTransform>().localPosition;
    //        if (boosterDropzone.tokenNumber > tokenCost) {
    //            boosterIsActive = true;
    //		} else {
    //            boosterIsActive = false;
    //		}

    //	}

    //	public void Start(){

    //		Debug.Log (basePosition);
    //		//defaultParent = this.transform.parent;
    //	}

    //	public void Update(){
    //		if (boosterDropzone.tokenNumber >= tokenCost) {
    //            boosterIsActive = true;
    //		} else {
    //            boosterIsActive = false;
    //		}
    //	}

    //	public void OnBeginDrag(PointerEventData eventData){
    //		//Debug.Log ("onbegindrag");
    //		Color color = new Vector4(0.5f,0.5f,0.5f,0.6f);
    //		boosterDropzone.setColor (color);
    //		if (boosterIsActive)
    //        {
    //		GetComponent<CanvasGroup> ().blocksRaycasts = false;
    //		}
    //	}

    //	public void OnDrag(PointerEventData eventData){
    //		//Debug.Log (eventData.position);
    //		if (boosterIsActive) {
    //			this.transform.position = eventData.position;

    //		}
    //	}

    //	public void OnEndDrag(PointerEventData eventData){
    //		//Debug.Log ("Onenddrag");
    //		Color color = new Vector4(0f,0f,0f,0f);
    //		boosterDropzone.setColor (color);
    //		if (boosterIsActive) {
    //			GetComponent<CanvasGroup> ().blocksRaycasts = true;
    //		}
    //	}

    //	public bool IsActive(){
    //		return boosterIsActive;
    //	}
}
