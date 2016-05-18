using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoosterDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public int tokenNumber=100;
	private Image image;
	private BoosterController boosterController;
	Color baseColor;

	public void Awake(){
		image = GetComponent<Image> ();
        boosterController = (BoosterController)FindObjectOfType(typeof(BoosterController));
		baseColor= image.color;
	}

	public void OnPointerEnter(PointerEventData eventData){
	}

	public void OnPointerExit(PointerEventData eventData){
		
	}

	public void OnDrop(PointerEventData eventData){
		image.color = baseColor;
		Debug.Log (eventData.pointerDrag.name + " was dropped to " +gameObject.name);

		BoosterUI bd = eventData.pointerDrag.GetComponent<BoosterUI> ();
		if (bd.IsAvailable()) {
			if (bd != null) {
				tokenNumber -= bd.tokenCost;
                //string BoosterName = eventData.pointerDrag.name;
                //boosterController.HandleBoosters (BoosterName, bd);
                boosterController.HandleBoosters(bd);
                //bd.transform.localPosition = bd.basePosition;
                Debug.Log (tokenNumber+" tokens left");
                bd.boosterIsAvailable = false;
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
