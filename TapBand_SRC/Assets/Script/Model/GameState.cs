﻿using UnityEngine;
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
        DailyEvent = new DailyEventState();
    }
    #endregion

    public CurrencyState Currency { get; private set; }
    public IList<MerchState> MerchStates { get; private set; }
    public IList<MerchSlotState> MerchSlotStates { get; private set; }
    public ConcertState Concert { get; private set; }
    public EquipmentState Equipment { get; private set; }
    public DailyEventState DailyEvent { get; private set; }

    #region Overridden functions for loading/saving
    protected override void LoadData(MemoryStream ms)
    {
        IFormatter formatter = new BinaryFormatter();
        GameState gs = (GameState)formatter.Deserialize(ms);

        Currency = gs.Currency == null ? new CurrencyState() : gs.Currency;
        MerchStates = gs.MerchStates == null ? new List<MerchState>() : gs.MerchStates;
        MerchSlotStates = gs.MerchSlotStates == null ? new List<MerchSlotState>() : gs.MerchSlotStates;
        Concert = gs.Concert == null ? new ConcertState() : gs.Concert;
        Equipment = gs.Equipment == null ? new EquipmentState() : gs.Equipment;
        DailyEvent = gs.DailyEvent == null ? new DailyEventState() : gs.DailyEvent;

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
