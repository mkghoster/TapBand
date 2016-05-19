//using UnityEngine;
//using System.Collections;

//public class EquipmentController : MonoBehaviour {

//    private EquipmentUI equipmentUI;

//    void Awake()
//    {
//        equipmentUI = (EquipmentUI)FindObjectOfType(typeof(EquipmentUI));
//    }
        
//    void OnEnable()
//    {
//        equipmentUI.CurrentDrumEquipmentData += CurrentDrumEquipmentData;
//        equipmentUI.NextDrumEquipmentData += NextDrumEquipmentData;
//        equipmentUI.CurrentGuitarEquipmentData += CurrentGuitarEquipmentData;
//        equipmentUI.NextGuitarEquipmentData += NextGuitarEquipmentData;
//        equipmentUI.CurrentBassEquipmentData += CurrentBassEquipmentData;
//        equipmentUI.NextBassEquipmentData += NextBassEquipmentData;
//        equipmentUI.CurrentKeyboardEquipmentData += CurrentKeyboardEquipmentData;
//        equipmentUI.NextKeyboardEquipmentData += NextKeyboardEquipmentData;

//        equipmentUI.BuyBassEquipment += BuyBassEquipment;
//        equipmentUI.BuyDrumEquipment += BuyDrumEquipment;
//        equipmentUI.BuyGuitarEquipment += BuyGuitarEquipment;
//        equipmentUI.BuyKeyboardEquipment += BuyKeyboardEquipment;
//        equipmentUI.CanBuy += CanBuy;
//    }

//    void OnDisable()
//    {
//        equipmentUI.CurrentDrumEquipmentData += CurrentDrumEquipmentData;
//        equipmentUI.NextDrumEquipmentData += NextDrumEquipmentData;
//        equipmentUI.CurrentGuitarEquipmentData += CurrentGuitarEquipmentData;
//        equipmentUI.NextGuitarEquipmentData += NextGuitarEquipmentData;
//        equipmentUI.CurrentBassEquipmentData += CurrentBassEquipmentData;
//        equipmentUI.NextBassEquipmentData += NextBassEquipmentData;
//        equipmentUI.CurrentKeyboardEquipmentData += CurrentKeyboardEquipmentData;
//        equipmentUI.NextKeyboardEquipmentData += NextKeyboardEquipmentData;

//        equipmentUI.BuyBassEquipment += BuyBassEquipment;
//        equipmentUI.BuyDrumEquipment += BuyDrumEquipment;
//        equipmentUI.BuyGuitarEquipment += BuyGuitarEquipment;
//        equipmentUI.BuyKeyboardEquipment += BuyKeyboardEquipment;
//        equipmentUI.CanBuy += CanBuy;
//    }



//    private CharacterData CurrentDrumEquipmentData()
//    {
//        return GameState.instance.Equipment.CurrentDrumEquipment;
//    }

//    private CharacterData NextDrumEquipmentData()
//    {
//        if (GameState.instance.Equipment.CurrentDrumEquipment == null)
//        {
//            return GameData.instance.EquipmentDataList.Find(x => x.equipmentType == EquipmentType.DRUM); // finds the first
//        }
//        bool currentFound = false;
//        foreach (CharacterData data in GameData.instance.EquipmentDataList)
//        {
//            if (currentFound && data.equipmentType == EquipmentType.DRUM)
//            {
//                return data;
//            }

//            if (data.id == GameState.instance.Equipment.DrumEquipmentId)
//            {
//                currentFound = true;
//            }
//        }

//        // figure this out
//        return CurrentDrumEquipmentData();
//    }

//    //duplicate

//    private CharacterData CurrentGuitarEquipmentData()
//    {
//        return GameState.instance.Equipment.CurrentGuitarEquipment;
//    }

//    private CharacterData NextGuitarEquipmentData()
//    {
//        if (GameState.instance.Equipment.CurrentGuitarEquipment == null)
//        {
//            return GameData.instance.EquipmentDataList.Find(x => x.equipmentType == EquipmentType.GUITAR); // finds the first
//        }
//        bool currentFound = false;
//        foreach (CharacterData data in GameData.instance.EquipmentDataList)
//        {
//            if (currentFound && data.equipmentType == EquipmentType.GUITAR)
//            {
//                return data;
//            }

//            if (data.id == GameState.instance.Equipment.GuitarEquipmentId)
//            {
//                currentFound = true;
//            }
//        }

//        // figure this out
//        return CurrentGuitarEquipmentData();
//    }

//    //duplicate

//    private CharacterData CurrentBassEquipmentData()
//    {
//        return GameState.instance.Equipment.CurrentBassEquipment;
//    }

//    private CharacterData NextBassEquipmentData()
//    {
//        if (GameState.instance.Equipment.CurrentBassEquipment == null)
//        {
//            return GameData.instance.EquipmentDataList.Find(x => x.equipmentType == EquipmentType.BASS); // finds the first
//        }
//        bool currentFound = false;
//        foreach (CharacterData data in GameData.instance.EquipmentDataList)
//        {
//            if (currentFound && data.equipmentType == EquipmentType.BASS)
//            {
//                return data;
//            }

//            if (data.id == GameState.instance.Equipment.BassEquipmentId)
//            {
//                currentFound = true;
//            }
//        }

//        // figure this out
//        return CurrentBassEquipmentData();
//    }

//    //duplicate

//    private CharacterData CurrentKeyboardEquipmentData()
//    {
//        return GameState.instance.Equipment.CurrentKeyboardEquipment;
//    }

//    private CharacterData NextKeyboardEquipmentData()
//    {
//        if (GameState.instance.Equipment.CurrentKeyboardEquipment == null)
//        {
//            return GameData.instance.EquipmentDataList.Find(x => x.equipmentType == EquipmentType.KEYBOARD); // finds the first
//        }
//        bool currentFound = false;
//        foreach (CharacterData data in GameData.instance.EquipmentDataList)
//        {
//            if (currentFound && data.equipmentType == EquipmentType.KEYBOARD)
//            {
//                return data;
//            }

//            if (data.id == GameState.instance.Equipment.KeyboardEquipmentId)
//            {
//                currentFound = true;
//            }
//        }

//        // figure this out
//        return CurrentKeyboardEquipmentData();
//    }

//    private void BuyBassEquipment(CharacterData data)
//    {
//        GameState.instance.Equipment.BassEquipmentId = data.id;
//        if (EquipmentTransaction != null)
//        {
//            EquipmentTransaction(data);
//        }
//    }

//    private void BuyDrumEquipment(CharacterData data)
//    {
//        GameState.instance.Equipment.DrumEquipmentId = data.id;
//        if (EquipmentTransaction != null)
//        {
//            EquipmentTransaction(data);
//        }
//    }

//    private void BuyGuitarEquipment(CharacterData data)
//    {
//        GameState.instance.Equipment.GuitarEquipmentId = data.id;
//        if (EquipmentTransaction != null)
//        {
//            EquipmentTransaction(data);
//        }
//    }

//    private void BuyKeyboardEquipment(CharacterData data)
//    {
//        GameState.instance.Equipment.KeyboardEquipmentId = data.id;
//        if (EquipmentTransaction != null)
//        {
//            EquipmentTransaction(data);
//        }
//    }

//    private bool CanBuy(int price)
//    {
//        return price <= GameState.instance.Currency.Coins;
//    }

//    public delegate void EquipmentTransactionEvent(CharacterData equipment);
//    public event EquipmentTransactionEvent EquipmentTransaction;

//    // Use this for initialization
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//}
