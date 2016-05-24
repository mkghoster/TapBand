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

    #region Private fields
    private List<SongData> songDataList;
    private List<ConcertData> concertDataList;
    private List<TourData> tourDataList;
    private List<EquipmentData> equipmentDataList;
    private List<MerchData>[] merchDataLists;
    private List<MerchSlotData> merchSlotDataList;
    private List<GeneralDataRecord> generalDataList;

    [System.NonSerialized]
    private GeneralData generalDatas;
    #endregion

    public GameData()
    {
        generalDatas = new GeneralData();
    }

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

    public List<MerchSlotData> MerchSlotDataList
    {
        get
        {
            return merchSlotDataList;
        }

        set
        {
            merchSlotDataList = value;
        }
    }

    public List<MerchData>[] MerchDataLists
    {
        get
        {
            return merchDataLists;
        }

        set
        {
            merchDataLists = value;
        }
    }

    public List<GeneralDataRecord> GeneralDataList
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

    public GeneralDataRecord FindGeneralDataByName(string name)
    {
        foreach (GeneralDataRecord data in generalDataList)
        {
            if (data.name == name)
            {
                return data;
            }
        }

        return null;
    }

    public GeneralData GeneralDatas
    {
        get
        {
            return generalDatas;
        }
    }

    public List<MerchData> GetMerchDataByType(MerchType type)
    {
        return merchDataLists[(int)type-1];
    }

    #region Overridden functions for loading/saving
    protected override void LoadData(MemoryStream ms)
    {
        IFormatter formatter = new BinaryFormatter();
        GameData gd = (GameData)formatter.Deserialize(ms);

        this.songDataList = gd.songDataList;
        this.concertDataList = gd.concertDataList;
        this.tourDataList = gd.tourDataList;
        this.merchDataLists = gd.merchDataLists;
        this.merchSlotDataList = gd.merchSlotDataList;
        this.equipmentDataList = gd.equipmentDataList;
        this.generalDataList = gd.generalDataList;

        generalDatas.Init();
    }

    public override string GetFileName()
    {
        return GAME_DATA_PATH;
    }
    #endregion
}
