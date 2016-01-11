using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour {
	public delegate EquipmentData EquipmentDataEvent();
	public event EquipmentDataEvent CurrentDrumEquipmentData;
	public event EquipmentDataEvent CurrentGuitarEquipmentData;
	public event EquipmentDataEvent CurrentBassEquipmentData;
	public event EquipmentDataEvent CurrentKeyboardEquipmentData;
	
	public event EquipmentDataEvent NextDrumEquipmentData;
	public event EquipmentDataEvent NextGuitarEquipmentData;
	public event EquipmentDataEvent NextBassEquipmentData;
	public event EquipmentDataEvent NextKeyboardEquipmentData;
	
	public delegate void BuyEquipmentEvent(EquipmentData data);
	public event BuyEquipmentEvent BuyDrumEquipment;
	public event BuyEquipmentEvent BuyGuitarEquipment;
	public event BuyEquipmentEvent BuyBassEquipment;
	public event BuyEquipmentEvent BuyKeyboardEquipment;
	
	public delegate bool CanBuyEvent(int price);
	public event CanBuyEvent CanBuy;
	
	public GameObject drumPanel, guitarPanel, bassPanel, keyboardPanel;

	void OnGUI()
	{
		if (CurrentDrumEquipmentData != null)
		{
			EquipmentData drumData = CurrentDrumEquipmentData();
			if (drumData != null)
			{
				GetTextComponentOfChild(drumPanel, "CurrentDrumHolder").text = drumData.name;
				GetTextComponentOfChild(drumPanel, "CurrentDrumsBenefitsHolder").text = drumData.tapMultiplier.ToString();
			}
		}
		
		GetButtonComponentOfChild(drumPanel, "DrumsUpgradeButton").interactable = false;
		
		if (NextDrumEquipmentData != null)
		{
			EquipmentData drumData = NextDrumEquipmentData();
			if (drumData != null)
			{
				GetButtonTextComponentOfChild(drumPanel, "DrumsUpgradeButton").text = "Buy for " + drumData.upgradeCost;
                //NextBoostProperties
                GetTextComponentOfChild(drumPanel, "NextDrumsBenefitsHolder").text = "It'll give " + drumData.tapMultiplier+ " multiplier";
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(drumPanel, "DrumsUpgradeButton");
					buyButton.interactable = CanBuy(drumData.upgradeCost);
					// FIXME: temporary solution
					buyButton.onClick.RemoveAllListeners();
					buyButton.onClick.AddListener(() => BuyDrumEquipment(drumData));
				}
			}
			
		}
		
		// this duplicate is not nice at all, I'll fix this later
		
		if (CurrentGuitarEquipmentData != null)
		{
			EquipmentData guitarData = CurrentGuitarEquipmentData();
			if (guitarData != null)
			{
				GetTextComponentOfChild(guitarPanel, "CurrentGuitarHolder").text = guitarData.name;
				GetTextComponentOfChild(guitarPanel, "CurrentGuitarBenefitsHolder").text = guitarData.tapMultiplier.ToString();
			}
		}
		
		GetButtonComponentOfChild(guitarPanel, "GuitarsUpgradeButton").interactable = false;
		
		if (NextGuitarEquipmentData != null)
		{
			EquipmentData guitarData = NextGuitarEquipmentData();
			if (guitarData != null)
			{
				GetButtonTextComponentOfChild(guitarPanel, "GuitarsUpgradeButton").text = "Buy for " + guitarData.upgradeCost;
				GetTextComponentOfChild(guitarPanel, "NextGuitarBenefitsHolder").text = "It'll give " + guitarData.tapMultiplier + " multiplier";
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(guitarPanel, "GuitarsUpgradeButton");
					buyButton.interactable = CanBuy(guitarData.upgradeCost);
					// FIXME: temporary solution
					buyButton.onClick.RemoveAllListeners();
					buyButton.onClick.AddListener(() => BuyGuitarEquipment(guitarData));
				}
			}
			
		}

		// this duplicate is not nice at all, I'll fix this later

		if (CurrentBassEquipmentData != null)
		{
			EquipmentData bassData = CurrentBassEquipmentData();
			if (bassData != null)
			{
				GetTextComponentOfChild(bassPanel, "CurrentBassHolder").text = bassData.name;
				GetTextComponentOfChild(bassPanel, "CurrentBassBenefitsHolder").text = bassData.tapMultiplier.ToString();
			}
		}
		
		GetButtonComponentOfChild(bassPanel, "BassUpgradeButton").interactable = false;
		
		if (NextBassEquipmentData != null)
		{
			EquipmentData bassData = NextBassEquipmentData();
			if (bassData != null)
			{
				GetButtonTextComponentOfChild(bassPanel, "BassUpgradeButton").text = "Buy for " + bassData.upgradeCost;
				GetTextComponentOfChild(bassPanel, "NextBassBenefitsHolder").text = "It'll give " + bassData.tapMultiplier + " multiplier";
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(bassPanel, "BassUpgradeButton");
					buyButton.interactable = CanBuy(bassData.upgradeCost);
					// FIXME: temporary solution
					buyButton.onClick.RemoveAllListeners();
					buyButton.onClick.AddListener(() => BuyBassEquipment(bassData));
				}
			}
			
		}

		// this duplicate is not nice at all, I'll fix this later

		if (CurrentKeyboardEquipmentData != null)
		{
			EquipmentData keyboardData = CurrentKeyboardEquipmentData();
			if (keyboardData != null)
			{
				GetTextComponentOfChild(keyboardPanel, "CurrentKeyboardHolder").text = keyboardData.name;
				GetTextComponentOfChild(keyboardPanel, "CurrentKeyboardBenefitsHolder").text = keyboardData.tapMultiplier.ToString();
			}
		}
		
		GetButtonComponentOfChild(keyboardPanel, "KeyboardUpgradeButton").interactable = false;
		
		if (NextKeyboardEquipmentData != null)
		{
			EquipmentData keyboardData = NextKeyboardEquipmentData();
			if (keyboardData != null)
			{
				GetButtonTextComponentOfChild(keyboardPanel, "KeyboardUpgradeButton").text = "Buy for " + keyboardData.upgradeCost;
				GetTextComponentOfChild(keyboardPanel, "NextKeyboardBenefitsHolder").text = "It'll give " + keyboardData.tapMultiplier + " multiplier";
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(keyboardPanel, "KeyboardUpgradeButton");
					buyButton.interactable = CanBuy(keyboardData.upgradeCost);
					// FIXME: temporary solution
					buyButton.onClick.RemoveAllListeners();
					buyButton.onClick.AddListener(() => BuyKeyboardEquipment(keyboardData));
				}
			}
			
		}


	}

	private Text GetTextComponentOfChild(GameObject parent, string childName)
	{
		return parent.transform.Find(childName).gameObject.GetComponent<Text>();
	}
	
	private Button GetButtonComponentOfChild(GameObject parent, string childName)
	{
		return parent.transform.Find(childName).gameObject.GetComponent<Button>();
	}
	
	private Text GetButtonTextComponentOfChild(GameObject parent, string childName)
	{
		return GetButtonComponentOfChild(parent, childName).transform.Find("Text").GetComponent<Text>();
	}

}
