using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System;

[System.Serializable]
public class GameData : LoadableData
{
    public const string GAME_DATA_PATH = "gamedata";

    #region Singleton access
    private static GameData _instance;
    public static GameData instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameStateHolder>().gameData;
            }
            return _instance;
        }
    }
    
    // TODO: solve its protection level issues in RawGameDataLoader
    /*
    private GameData()
    {

    }
    */
    #endregion

    private List<SongData> songDataList;
    private List<ConcertData> concertDataList;
    private List<TourData> tourDataList;
    private List<EquipmentData> equipmentDataList;
    private List<MerchData> merchDataList;
    private List<GeneralData> generalDataList;

    public List<SongData> SongDataList
    {
        get
        {
            return songDataList;
        }

        set
        {
            songDataList = value;
        }
    }

    public List<ConcertData> ConcertDataList
    {
        get
        {
            return concertDataList;
        }

        set
        {
            concertDataList = value;
        }
    }

    public List<TourData> TourDataList
    {
        get
        {
            return tourDataList;
        }

        set
        {
            tourDataList = value;
        }
    }

    public List<EquipmentData> EquipmentDataList
    {
        get
        {
            return equipmentDataList;
        }

        set
        {
            equipmentDataList = value;
        }
    }

    public List<MerchData> MerchDataList
    {
        get
        {
            return merchDataList;
        }

        set
        {
            merchDataList = value;
        }
    }

    public List<GeneralData> GeneralDataList
    {
        get
        {
            return generalDataList;
        }

        set
        {
            generalDataList = value;
        }
    }

    #region Overridden functions for loading/saving
    protected override void LoadData(MemoryStream ms)
    {
        IFormatter formatter = new BinaryFormatter();
        GameData gd = (GameData)formatter.Deserialize(ms);

        this.songDataList = gd.songDataList;
        this.concertDataList = gd.concertDataList;
        this.tourDataList = gd.tourDataList;
        this.merchDataList = gd.merchDataList;
        this.equipmentDataList = gd.equipmentDataList;
        this.generalDataList = gd.generalDataList;
    }

    public override string GetFileName()
    {
        return GAME_DATA_PATH;
    }
    #endregion

}