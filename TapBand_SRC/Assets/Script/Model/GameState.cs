using UnityEngine;
using System.Collections;
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

    private GameState()
    {
        currencyState = new CurrencyState();
        merchState = new MerchState();
        concertState = new ConcertState();
        equipmentState = new EquipmentState();
    }
    #endregion

    private CurrencyState currencyState;
    private MerchState merchState;
    private ConcertState concertState;
    private EquipmentState equipmentState;

    
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

    public MerchState Merch
    {
        get
        {
            return merchState;
        }

        set
        {
            merchState = value;
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
        GameState gd = (GameState)formatter.Deserialize(ms);
        
        this.currencyState = gd.currencyState == null ? new CurrencyState() : gd.currencyState;
        this.merchState = gd.merchState == null ? new MerchState() : gd.merchState;
		this.concertState = gd.concertState == null ? new ConcertState() : gd.concertState;
        this.equipmentState = gd.equipmentState == null ? new EquipmentState() : gd.equipmentState;
    }

    public override string GetFileName()
    {
        return "gamestate";
    }
    #endregion
}
