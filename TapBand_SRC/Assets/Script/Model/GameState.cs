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
    #endregion

    private CurrencyState currencyState;
    private TourState tourState;
    private MerchState merchState;
    // TODO folytatni, pl:
    // private ConcertState concertState;

    public TourState Tour
    {
        get
        {
            return tourState;
        }

        set
        {
            tourState = value;
        }
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

    #region Overridden functions for loading/saving
    protected override void LoadData(MemoryStream ms)
    {
        IFormatter formatter = new BinaryFormatter();
        GameState gd = (GameState)formatter.Deserialize(ms);
        
        this.tourState = gd.tourState == null ? new TourState() : gd.tourState;
        this.currencyState = gd.currencyState == null ? new CurrencyState() : gd.currencyState;
        this.merchState = gd.merchState == null ? new MerchState() : gd.merchState;
    }

    public override string GetFileName()
    {
        return "gamestate";
    }
    #endregion
}
