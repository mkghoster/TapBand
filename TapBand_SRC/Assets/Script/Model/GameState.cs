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
        MerchStates = new List<MerchState>();
        MerchSlotStates = new List<MerchSlotState>();
        Concert = new ConcertState();
        Equipment = new EquipmentState();
    }
    #endregion

    public CurrencyState Currency { get; private set; }
    public IList<MerchState> MerchStates { get; private set; }
    public IList<MerchSlotState> MerchSlotStates { get; private set; }
    public ConcertState Concert { get; private set; }
    public EquipmentState Equipment { get; private set; }

    #region Overridden functions for loading/saving
    protected override void LoadData(MemoryStream ms)
    {
        IFormatter formatter = new BinaryFormatter();
        GameState gd = (GameState)formatter.Deserialize(ms);

        Currency = gd.Currency == null ? new CurrencyState() : gd.Currency;
        MerchStates = gd.MerchStates == null ? new List<MerchState>() : gd.MerchStates;
        MerchSlotStates = gd.MerchSlotStates == null ? new List<MerchSlotState>() : gd.MerchSlotStates;
        Concert = gd.Concert == null ? new ConcertState() : gd.Concert;
        Equipment = gd.Equipment == null ? new EquipmentState() : gd.Equipment;

        Init();
    }

    public override string GetFileName()
    {
        return "gamestate";
    }
    #endregion

    public void Init()
    {
        if (MerchStates.Count == 0)
        {
            for (int i = 1; i <= (int)MerchType.NUM_OF_MERCH_TYPES; i++)
            {
                MerchStates.Add(new MerchState((MerchType)i));
            }
        }
        if (MerchSlotStates.Count == 0)
        {
            for (int i = 1; i <= 4; i++)
            {
                MerchSlotStates.Add(new MerchSlotState(i));
            }
        }

        for (int i = 0; i < MerchStates.Count; i++)
        {
            MerchStates[i].Init();
        }
        for (int i = 0; i < MerchSlotStates.Count; i++)
        {
            MerchSlotStates[i].Init();
        }
    }
}
