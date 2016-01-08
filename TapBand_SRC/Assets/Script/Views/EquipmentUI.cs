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
				GetTextComponentOfChild(drumPanel, "CurrentDrumHolder").text = drumData.level.ToString();
				GetTextComponentOfChild(drumPanel, "CurrentDrumsBenefitsHolder").text = drumData.name;
				GetTextComponentOfChild(drumPanel, "NextDrumsBenefitsHolder").text = drumData.tapMultiplier.ToString();
			}
		}
		
		GetButtonComponentOfChild(drumPanel, "DrumsUpgradeButton").interactable = false;
		
		if (NextDrumEquipmentData != null)
		{
			EquipmentData drumData = NextDrumEquipmentData();
			if (drumData != null)
			{
				GetButtonTextComponentOfChild(drumPanel, "DrumsUpgradeButton").text = "Buy " + drumData.name;
				GetTextComponentOfChild(drumPanel, "NextBoostProperties").text = "It'll give " + drumData.tapMultiplier;
				
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
				GetTextComponentOfChild(guitarPanel, "CurrentGuitarHolder").text = guitarData.level.ToString();
				GetTextComponentOfChild(guitarPanel, "CurrentGuitarBenefitsHolder").text = guitarData.name;
				GetTextComponentOfChild(guitarPanel, "NextGuitarBenefitsHolder").text = guitarData.tapMultiplier.ToString();
			}
		}
		
		GetButtonComponentOfChild(guitarPanel, "GuitarsUpgradeButton").interactable = false;
		
		if (NextGuitarEquipmentData != null)
		{
			EquipmentData guitarData = NextGuitarEquipmentData();
			if (guitarData != null)
			{
				GetButtonTextComponentOfChild(guitarPanel, "GuitarsUpgradeButton").text = "Buy " + guitarData.name;
				GetTextComponentOfChild(guitarPanel, "NextBoostProperties").text = "It'll give " + guitarData.tapMultiplier;
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(guitarPanel, "GuitarsUpgradeButton");
					buyButton.interactable = CanBuy(guitarData.upgradeCost);
					// FIXME: temporary solution
					buyButton.onClick.RemoveAllListeners();
					buyButton.onClick.AddListener(() => BuyDrumEquipment(guitarData));
				}
			}
			
		}

		// this duplicate is not nice at all, I'll fix this later

		if (CurrentBassEquipmentData != null)
		{
			EquipmentData bassData = CurrentBassEquipmentData();
			if (bassData != null)
			{
				GetTextComponentOfChild(bassPanel, "CurrentBassHolder").text = bassData.level.ToString();
				GetTextComponentOfChild(bassPanel, "CurrentBassBenefitsHolder").text = bassData.name;
				GetTextComponentOfChild(bassPanel, "NextBassBenefitsHolder").text = bassData.tapMultiplier.ToString();
			}
		}
		
		GetButtonComponentOfChild(bassPanel, "BassUpgradeButton").interactable = false;
		
		if (NextBassEquipmentData != null)
		{
			EquipmentData bassData = NextBassEquipmentData();
			if (bassData != null)
			{
				GetButtonTextComponentOfChild(bassPanel, "BassUpgradeButton").text = "Buy " + bassData.name;
				GetTextComponentOfChild(bassPanel, "NextBoostProperties").text = "It'll give " + bassData.tapMultiplier;
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(bassPanel, "BassUpgradeButton");
					buyButton.interactable = CanBuy(bassData.upgradeCost);
					// FIXME: temporary solution
					buyButton.onClick.RemoveAllListeners();
					buyButton.onClick.AddListener(() => BuyDrumEquipment(bassData));
				}
			}
			
		}

		// this duplicate is not nice at all, I'll fix this later

		if (CurrentKeyboardEquipmentData != null)
		{
			EquipmentData keyboardData = CurrentKeyboardEquipmentData();
			if (keyboardData != null)
			{
				GetTextComponentOfChild(keyboardPanel, "CurrentKeyboardHolder").text = keyboardData.level.ToString();
				GetTextComponentOfChild(keyboardPanel, "CurrentKeyboardBenefitsHolder").text = keyboardData.name;
				GetTextComponentOfChild(keyboardPanel, "NextKeyboardBenefitsHolder").text = keyboardData.tapMultiplier.ToString();
			}
		}
		
		GetButtonComponentOfChild(keyboardPanel, "KeyboardUpgradeButton").interactable = false;
		
		if (NextKeyboardEquipmentData != null)
		{
			EquipmentData keyboardData = NextKeyboardEquipmentData();
			if (keyboardData != null)
			{
				GetButtonTextComponentOfChild(keyboardPanel, "KeyboardUpgradeButton").text = "Buy " + keyboardData.name;
				GetTextComponentOfChild(keyboardPanel, "NextBoostProperties").text = "It'll give " + keyboardData.tapMultiplier;
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(keyboardPanel, "KeyboardUpgradeButton");
					buyButton.interactable = CanBuy(keyboardData.upgradeCost);
					// FIXME: temporary solution
					buyButton.onClick.RemoveAllListeners();
					buyButton.onClick.AddListener(() => BuyDrumEquipment(keyboardData));
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
