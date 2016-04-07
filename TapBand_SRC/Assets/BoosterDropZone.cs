using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoosterDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public int tokenNumber=40;
	private Image image;
	private BoosterUI boosterUI;
	Color baseColor;

	public void Awake(){
		image = GetComponent<Image> ();
		boosterUI = (BoosterUI)FindObjectOfType(typeof(BoosterUI));
		baseColor= image.color;
	}

	public void OnPointerEnter(PointerEventData eventData){
	}

	public void OnPointerExit(PointerEventData eventData){
		
	}

	public void OnDrop(PointerEventData eventData){
		image.color = baseColor;
		Debug.Log (eventData.pointerDrag.name + " was dropped to " +gameObject.name);

		BoosterDraggable bd = eventData.pointerDrag.GetComponent<BoosterDraggable> ();
		if (bd.IsActive()) {
			if (bd != null) {
				tokenNumber -= bd.tokenCost;
				string BoosterName = eventData.pointerDrag.name;
				boosterUI.HandleBoosters (BoosterName);
				bd.transform.localPosition = bd.basePosition;
				Debug.Log (tokenNumber);
				if (tokenNumber <= 0) 
					{
						eventData.pointerDrag.SetActive (false);
					}
			}
		} 
	}

	public void setColor(Vector4 color){
		image.color = color;
	}

}
