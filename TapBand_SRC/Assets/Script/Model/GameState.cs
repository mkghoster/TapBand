using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using System.IO;
using System.Collections.Generic;

[Serializable]
public class GameState : LoadableData
{

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
        Currency = new CurrencyState();
        Merch = new MerchState();
        Concert = new ConcertState();
        Equipment = new EquipmentState();
    }
    #endregion

    public CurrencyState Currency { get; private set; }
    public MerchState Merch { get; private set; }
    public ConcertState Concert { get; private set; }
    public EquipmentState Equipment { get; private set; }

    #region Overridden functions for loading/saving
    protected override void LoadData(MemoryStream ms)
    {
        IFormatter formatter = new BinaryFormatter();
        GameState gd = (GameState)formatter.Deserialize(ms);

        Currency = gd.Currency == null ? new CurrencyState() : gd.Currency;
        Merch = gd.Merch == null ? new MerchState() : gd.Merch;
        Concert = gd.Concert == null ? new ConcertState() : gd.Concert;
        Equipment = gd.Equipment == null ? new EquipmentState() : gd.Equipment;                
    }

    public override string GetFileName()
    {
        return "gamestate";
    }
    #endregion
}
