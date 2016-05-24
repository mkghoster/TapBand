using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.IO;

[System.Serializable]
public class GameState : LoadableData {

    #region Singleton access
    private static GameState _instance;
    public static GameState instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameStateHolder>().gameState;
            }
            return _instance;
        }
    }
    #endregion

    #region Private fields
    private CurrencyState currencyState;
    private List<MerchState> merchStates;
    private List<MerchSlotState> merchSlotStates;
    private ConcertState concertState;
    private EquipmentState equipmentState;
    #endregion

    [SerializeField]
    private GameState()
    {
        currencyState = new CurrencyState();
        merchStates = new List<MerchState>();
        merchSlotStates = new List<MerchSlotState>();
        concertState = new ConcertState();
        equipmentState = new EquipmentState();
    }

    public CurrencyState Currency
    {
        get
        {
            return currencyState;
        }

        set
        {
            currencyState = value;
        }
    }

    public List<MerchState> MerchStates
    {
        get
        {
            return merchStates;
        }

        set
        {
            merchStates = value;
        }
    }

    public List<MerchSlotState> MerchSlotStates
    {
        get
        {
            return merchSlotStates;
        }

        set
        {
            merchSlotStates = value;
        }
    }

    public ConcertState Concert
	{
		get
		{
			return concertState;
		}
		
		set
		{
			concertState = value;
		}
	}

    public EquipmentState Equipment
    {
        get
        {
            return equipmentState;
        }

        set
        {
            equipmentState = value;
        }
    }

    #region Overridden functions for loading/saving
    protected override void LoadData(MemoryStream ms)
    {
        IFormatter formatter = new BinaryFormatter();
        GameState gs = (GameState)formatter.Deserialize(ms);
        
        this.currencyState = gs.currencyState == null ? new CurrencyState() : gs.currencyState;
        this.merchStates = gs.merchStates == null ? new List<MerchState>() : gs.merchStates;
        this.merchSlotStates = gs.merchSlotStates == null ? new List<MerchSlotState>() : gs.merchSlotStates;
		this.concertState = gs.concertState == null ? new ConcertState() : gs.concertState;
        this.equipmentState = gs.equipmentState == null ? new EquipmentState() : gs.equipmentState;

        Init();

        this.currencyState.SynchronizeRealCurrencyAndScreenCurrency();
    }

    public override string GetFileName()
    {
        return "gamestate";
    }
    #endregion

    public void Init()
    {
        if (merchStates.Count == 0)
        {
            for (int i = 1; i <= (int)MerchType.NUM_OF_MERCH_TYPES; i++)
            {
                merchStates.Add(new MerchState((MerchType)i));
            }
        }
        if (merchSlotStates.Count == 0)
        {
            for (int i = 1; i <= 4; i++)
            {
                merchSlotStates.Add(new MerchSlotState(i));
            }
        }

        for (int i = 0; i < merchStates.Count; i++)
        {
            merchStates[i].Init();
        }
        for (int i = 0; i < merchSlotStates.Count; i++)
        {
            merchSlotStates[i].Init();
        }
    }
}
