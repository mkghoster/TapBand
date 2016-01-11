using UnityEngine;
using System.Collections;

public class EquipmentController : MonoBehaviour {

    private EquipmentUI equipmentUI;

    void Awake()
    {
        equipmentUI = (EquipmentUI)FindObjectOfType(typeof(EquipmentUI));
    }

    void OnEnable()
    {
        equipmentUI.CurrentDrumEquipmentData += CurrentDrumEquipmentData;
        equipmentUI.NextDrumEquipmentData += NextDrumEquipmentData;
        equipmentUI.CurrentGuitarEquipmentData += CurrentGuitarEquipmentData;
        equipmentUI.NextGuitarEquipmentData += NextGuitarEquipmentData;
        equipmentUI.CurrentBassEquipmentData += CurrentBassEquipmentData;
        equipmentUI.NextBassEquipmentData += NextBassEquipmentData;
        equipmentUI.CurrentKeyboardEquipmentData += CurrentKeyboardEquipmentData;
        equipmentUI.NextKeyboardEquipmentData += NextKeyboardEquipmentData;

        equipmentUI.BuyBassEquipment += BuyBassEquipment;
        equipmentUI.BuyDrumEquipment += BuyDrumEquipment;
        equipmentUI.BuyGuitarEquipment += BuyGuitarEquipment;
        equipmentUI.BuyKeyboardEquipment += BuyKeyboardEquipment;
        equipmentUI.CanBuy += CanBuy;
    }

    void OnDisable()
    {
        equipmentUI.CurrentDrumEquipmentData += CurrentDrumEquipmentData;
        equipmentUI.NextDrumEquipmentData += NextDrumEquipmentData;
        equipmentUI.CurrentGuitarEquipmentData += CurrentGuitarEquipmentData;
        equipmentUI.NextGuitarEquipmentData += NextGuitarEquipmentData;
        equipmentUI.CurrentBassEquipmentData += CurrentBassEquipmentData;
        equipmentUI.NextBassEquipmentData += NextBassEquipmentData;
        equipmentUI.CurrentKeyboardEquipmentData += CurrentKeyboardEquipmentData;
        equipmentUI.NextKeyboardEquipmentData += NextKeyboardEquipmentData;

        equipmentUI.BuyBassEquipment += BuyBassEquipment;
        equipmentUI.BuyDrumEquipment += BuyDrumEquipment;
        equipmentUI.BuyGuitarEquipment += BuyGuitarEquipment;
        equipmentUI.BuyKeyboardEquipment += BuyKeyboardEquipment;
        equipmentUI.CanBuy += CanBuy;
    }

    private EquipmentData CurrentDrumEquipmentData()
    {
        return GameState.instance.Equipment.CurrentDrumEquipment;
    }

    private EquipmentData NextDrumEquipmentData()
    {
        if (GameState.instance.Equipment.CurrentDrumEquipment == null)
        {
            return GameData.instance.EquipmentDataList.Find(x => x.equipmentType == EquipmentType.DRUM); // finds the first
        }
        bool currentFound = false;
        foreach (EquipmentData data in GameData.instance.EquipmentDataList)
        {
            if (currentFound && data.equipmentType == EquipmentType.DRUM)
            {
                return data;
            }

            if (data.id == GameState.instance.Equipment.DrumEquipmentId)
            {
                currentFound = true;
            }
        }

        // figure this out
        return CurrentDrumEquipmentData();
    }

    //duplicate

    private EquipmentData CurrentGuitarEquipmentData()
    {
        return GameState.instance.Equipment.CurrentGuitarEquipment;
    }

    private EquipmentData NextGuitarEquipmentData()
    {
        if (GameState.instance.Equipment.CurrentGuitarEquipment == null)
        {
            return GameData.instance.EquipmentDataList.Find(x => x.equipmentType == EquipmentType.GUITAR); // finds the first
        }
        bool currentFound = false;
        foreach (EquipmentData data in GameData.instance.EquipmentDataList)
        {
            if (currentFound && data.equipmentType == EquipmentType.GUITAR)
            {
                return data;
            }

            if (data.id == GameState.instance.Equipment.GuitarEquipmentId)
            {
                currentFound = true;
            }
        }

        // figure this out
        return CurrentGuitarEquipmentData();
    }

    //duplicate

    private EquipmentData CurrentBassEquipmentData()
    {
        return GameState.instance.Equipment.CurrentBassEquipment;
    }

    private EquipmentData NextBassEquipmentData()
    {
        if (GameState.instance.Equipment.CurrentBassEquipment == null)
        {
            return GameData.instance.EquipmentDataList.Find(x => x.equipmentType == EquipmentType.BASS); // finds the first
        }
        bool currentFound = false;
        foreach (EquipmentData data in GameData.instance.EquipmentDataList)
        {
            if (currentFound && data.equipmentType == EquipmentType.BASS)
            {
                return data;
            }

            if (data.id == GameState.instance.Equipment.BassEquipmentId)
            {
                currentFound = true;
            }
        }

        // figure this out
        return CurrentBassEquipmentData();
    }

    //duplicate

    private EquipmentData CurrentKeyboardEquipmentData()
    {
        return GameState.instance.Equipment.CurrentKeyboardEquipment;
    }

    private EquipmentData NextKeyboardEquipmentData()
    {
        if (GameState.instance.Equipment.CurrentKeyboardEquipment == null)
        {
            return GameData.instance.EquipmentDataList.Find(x => x.equipmentType == EquipmentType.KEYBOARD); // finds the first
        }
        bool currentFound = false;
        foreach (EquipmentData data in GameData.instance.EquipmentDataList)
        {
            if (currentFound && data.equipmentType == EquipmentType.KEYBOARD)
            {
                return data;
            }

            if (data.id == GameState.instance.Equipment.KeyboardEquipmentId)
            {
                currentFound = true;
            }
        }

        // figure this out
        return CurrentKeyboardEquipmentData();
    }

    private void BuyBassEquipment(EquipmentData data)
    {
        GameState.instance.Equipment.BassEquipmentId = data.id;
        if (CoinTransaction != null)
        {
            CoinTransaction(-data.upgradeCost);
        }
    }

    private void BuyDrumEquipment(EquipmentData data)
    {
        GameState.instance.Equipment.DrumEquipmentId = data.id;
        if (CoinTransaction != null)
        {
            CoinTransaction(-data.upgradeCost);
        }
    }

    private void BuyGuitarEquipment(EquipmentData data)
    {
        GameState.instance.Equipment.GuitarEquipmentId = data.id;
        if (CoinTransaction != null)
        {
            CoinTransaction(-data.upgradeCost);
        }
    }

    private void BuyKeyboardEquipment(EquipmentData data)
    {
        GameState.instance.Equipment.KeyboardEquipmentId = data.id;
        if (CoinTransaction != null)
        {
            CoinTransaction(-data.upgradeCost);
        }
    }

    private bool CanBuy(int price)
    {
        return price <= GameState.instance.Currency.NumberOfCoins;
    }

    public delegate void ModifyCoinEvent(int price);
    public event ModifyCoinEvent CoinTransaction;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
