using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate CharacterData EquipmentDataEvent();
public delegate void BuyEquipmentEvent(CharacterData data);
public delegate bool CanBuyEvent(int price);

public class EquipmentUI : MonoBehaviour {
    public BuyEquipmentEvent OnEquipmentBought;

	public event EquipmentDataEvent CurrentDrumEquipmentData;
	public event EquipmentDataEvent CurrentGuitarEquipmentData;
	public event EquipmentDataEvent CurrentBassEquipmentData;
	public event EquipmentDataEvent CurrentKeyboardEquipmentData;
	
	public event EquipmentDataEvent NextDrumEquipmentData;
	public event EquipmentDataEvent NextGuitarEquipmentData;
	public event EquipmentDataEvent NextBassEquipmentData;
	public event EquipmentDataEvent NextKeyboardEquipmentData;
	

	public event BuyEquipmentEvent BuyDrumEquipment;
	public event BuyEquipmentEvent BuyGuitarEquipment;
	public event BuyEquipmentEvent BuyBassEquipment;
	public event BuyEquipmentEvent BuyKeyboardEquipment;
	
	
	public event CanBuyEvent CanBuy;
	
	public GameObject drumPanel, guitarPanel, bassPanel, keyboardPanel;

	void OnGUI()
	{
		if (CurrentDrumEquipmentData != null)
		{
			CharacterData drumData = CurrentDrumEquipmentData();
			if (drumData != null)
			{
				GetTextComponentOfChild(drumPanel, "CurrentDrumHolder").text = drumData.name;
				GetTextComponentOfChild(drumPanel, "CurrentDrumsBenefitsHolder").text = drumData.tapStrengthBonus.ToString();
			}
		}
		
		GetButtonComponentOfChild(drumPanel, "DrumsUpgradeButton").interactable = false;
		
		if (NextDrumEquipmentData != null)
		{
			CharacterData drumData = NextDrumEquipmentData();
			if (drumData != null)
			{
				GetButtonTextComponentOfChild(drumPanel, "DrumsUpgradeButton").text = "Buy for " + drumData.upgradeCost;
                //NextBoostProperties
                GetTextComponentOfChild(drumPanel, "NextDrumsBenefitsHolder").text = "It'll give " + drumData.tapStrengthBonus+ " multiplier";
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(drumPanel, "DrumsUpgradeButton");
					//buyButton.interactable = CanBuy(drumData.upgradeCost);
					// FIXME: temporary solution
					buyButton.onClick.RemoveAllListeners();
					buyButton.onClick.AddListener(() => BuyDrumEquipment(drumData));
				}
			}
			
		}
		
		// this duplicate is not nice at all, I'll fix this later
		
		if (CurrentGuitarEquipmentData != null)
		{
			CharacterData guitarData = CurrentGuitarEquipmentData();
			if (guitarData != null)
			{
				GetTextComponentOfChild(guitarPanel, "CurrentGuitarHolder").text = guitarData.name;
				GetTextComponentOfChild(guitarPanel, "CurrentGuitarBenefitsHolder").text = guitarData.tapStrengthBonus.ToString();
			}
		}
		
		GetButtonComponentOfChild(guitarPanel, "GuitarsUpgradeButton").interactable = false;
		
		if (NextGuitarEquipmentData != null)
		{
			CharacterData guitarData = NextGuitarEquipmentData();
			if (guitarData != null)
			{
				GetButtonTextComponentOfChild(guitarPanel, "GuitarsUpgradeButton").text = "Buy for " + guitarData.upgradeCost;
				GetTextComponentOfChild(guitarPanel, "NextGuitarBenefitsHolder").text = "It'll give " + guitarData.tapStrengthBonus + " multiplier";
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(guitarPanel, "GuitarsUpgradeButton");
					//buyButton.interactable = CanBuy(guitarData.upgradeCost);
					// FIXME: temporary solution
					buyButton.onClick.RemoveAllListeners();
					buyButton.onClick.AddListener(() => BuyGuitarEquipment(guitarData));
				}
			}
			
		}

		// this duplicate is not nice at all, I'll fix this later

		if (CurrentBassEquipmentData != null)
		{
			CharacterData bassData = CurrentBassEquipmentData();
			if (bassData != null)
			{
				GetTextComponentOfChild(bassPanel, "CurrentBassHolder").text = bassData.name;
				GetTextComponentOfChild(bassPanel, "CurrentBassBenefitsHolder").text = bassData.tapStrengthBonus.ToString();
			}
		}
		
		GetButtonComponentOfChild(bassPanel, "BassUpgradeButton").interactable = false;
		
		if (NextBassEquipmentData != null)
		{
			CharacterData bassData = NextBassEquipmentData();
			if (bassData != null)
			{
				GetButtonTextComponentOfChild(bassPanel, "BassUpgradeButton").text = "Buy for " + bassData.upgradeCost;
				GetTextComponentOfChild(bassPanel, "NextBassBenefitsHolder").text = "It'll give " + bassData.tapStrengthBonus + " multiplier";
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(bassPanel, "BassUpgradeButton");
					//buyButton.interactable = CanBuy(bassData.upgradeCost);
					// FIXME: temporary solution
					buyButton.onClick.RemoveAllListeners();
					buyButton.onClick.AddListener(() => BuyBassEquipment(bassData));
				}
			}
			
		}

		// this duplicate is not nice at all, I'll fix this later

		if (CurrentKeyboardEquipmentData != null)
		{
			CharacterData keyboardData = CurrentKeyboardEquipmentData();
			if (keyboardData != null)
			{
				GetTextComponentOfChild(keyboardPanel, "CurrentKeyboardHolder").text = keyboardData.name;
				GetTextComponentOfChild(keyboardPanel, "CurrentKeyboardBenefitsHolder").text = keyboardData.tapStrengthBonus.ToString();
			}
		}
		
		GetButtonComponentOfChild(keyboardPanel, "KeyboardUpgradeButton").interactable = false;
		
		if (NextKeyboardEquipmentData != null)
		{
			CharacterData keyboardData = NextKeyboardEquipmentData();
			if (keyboardData != null)
			{
				GetButtonTextComponentOfChild(keyboardPanel, "KeyboardUpgradeButton").text = "Buy for " + keyboardData.upgradeCost;
				GetTextComponentOfChild(keyboardPanel, "NextKeyboardBenefitsHolder").text = "It'll give " + keyboardData.tapStrengthBonus + " multiplier";
				
				if (CanBuy != null)
				{
					Button buyButton = GetButtonComponentOfChild(keyboardPanel, "KeyboardUpgradeButton");
					//buyButton.interactable = CanBuy(keyboardData.upgradeCost);
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
