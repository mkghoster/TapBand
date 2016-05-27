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
        DailyEvent = new DailyEventState();
    }
    #endregion

    public CurrencyState Currency { get; private set; }
    public MerchState Merch { get; private set; }
    public ConcertState Concert { get; private set; }
    public EquipmentState Equipment { get; private set; }
    public DailyEventState DailyEvent { get; private set; }

    #region Overridden functions for loading/saving
    protected override void LoadData(MemoryStream ms)
    {
        IFormatter formatter = new BinaryFormatter();
        GameState gs = (GameState)formatter.Deserialize(ms);

        Currency = gs.Currency == null ? new CurrencyState() : gs.Currency;
        Merch = gs.Merch == null ? new MerchState() : gs.Merch;
        Concert = gs.Concert == null ? new ConcertState() : gs.Concert;
        Equipment = gs.Equipment == null ? new EquipmentState() : gs.Equipment;
        DailyEvent = gs.DailyEvent == null ? new DailyEventState() : gs.DailyEvent;
    }

    public override string GetFileName()
    {
        return "gamestate";
    }
    #endregion
}
