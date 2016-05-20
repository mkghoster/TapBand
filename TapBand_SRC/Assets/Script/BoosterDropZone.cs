using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoosterDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public int tokenNumber=100;
	private Image image;
	private BoosterController boosterController;
    //TODO token bekötés
    //private CurrencyState currencyState;

    Color defaultColor;
    private int token;

    public void Awake(){
		image = GetComponent<Image> ();
        boosterController = (BoosterController)FindObjectOfType(typeof(BoosterController));
        defaultColor = image.color;
        //currencyState = GameState.instance.Currency;
        //token = currencyState.Tokens;     
    }

	public void OnPointerEnter(PointerEventData eventData){
	}

	public void OnPointerExit(PointerEventData eventData){
		
	}

	public void OnDrop(PointerEventData eventData){
		image.color = defaultColor;
        //Debug.Log (eventData.pointerDrag.name + " was dropped to " +gameObject.name);
        BoosterUI currentBooster = eventData.pointerDrag.GetComponent<BoosterUI> ();
		if (currentBooster.IsAvailable()) {
			if (currentBooster != null) {
				tokenNumber -= currentBooster.tokenCost;
                boosterController.HandleBoosters(currentBooster);
                Debug.Log (tokenNumber+" tokens left");
                currentBooster.boosterIsAvailable = false;
			}
		} 
	}

	public void setColor(Vector4 color){
		image.color = color;
	}

}
