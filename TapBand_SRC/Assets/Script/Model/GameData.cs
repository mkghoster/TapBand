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

    public IList<SongData> SongDataList { get; private set; }
    public IList<ConcertData> ConcertDataList { get; private set; }

    public IList<CharacterData> CharacterData1List { get; private set; }
    public IList<CharacterData> CharacterData2List { get; private set; }
    public IList<CharacterData> CharacterData3List { get; private set; }
    public IList<CharacterData> CharacterData4List { get; private set; }
    public IList<CharacterData> CharacterData5List { get; private set; }

    public IList<SkinData> SkinData1List { get; private set; }
    public IList<SkinData> SkinData2List { get; private set; }
    public IList<SkinData> SkinData3List { get; private set; }
    public IList<SkinData> SkinData4List { get; private set; }
    public IList<SkinData> SkinData5List { get; private set; }

    public IList<MerchData> MerchData1List { get; private set; }
    public IList<MerchData> MerchData2List { get; private set; }
    public IList<MerchData> MerchData3List { get; private set; }
    public IList<MerchData> MerchData4List { get; private set; }
    public IList<MerchData> MerchData5List { get; private set; }
    public IList<MerchData> MerchData6List { get; private set; }

    public IList<MerchSlotData> MerchSlotData { get; private set; }

    public IapData IapData { get; private set; }
    public GeneralData GeneralData { get; private set; }

    public IList<DroneRewardData> DroneRewardDataList { get; private set; }

    public IList<DailyRandomData> DailyRandomDataList { get; private set; }
    public IList<DailyStreakData> DailyStreakDataList { get; private set; }

    public BoosterData BoosterData { get; private set; }

    #region Overridden functions for loading/saving
    protected override void LoadData(MemoryStream ms)
    {
        IFormatter formatter = new BinaryFormatter();
        GameData gd = (GameData)formatter.Deserialize(ms);

        // nem lenne egyszerűbb staticcá tenni a loaddatát, és az instancet beállítani a deserializált cuccra?

        SongDataList = gd.SongDataList;
        ConcertDataList = gd.ConcertDataList;
        CharacterData1List = gd.CharacterData1List;
        CharacterData2List = gd.CharacterData2List;
        CharacterData3List = gd.CharacterData3List;
        CharacterData4List = gd.CharacterData4List;
        CharacterData5List = gd.CharacterData5List;

        MerchData1List = gd.MerchData1List;
        MerchData2List = gd.MerchData2List;
        MerchData3List = gd.MerchData3List;
        MerchData4List = gd.MerchData4List;
        MerchData5List = gd.MerchData5List;
        MerchData6List = gd.MerchData6List;

        MerchSlotData = gd.MerchSlotData;

        IapData = gd.IapData;
        GeneralData = gd.GeneralData;

        DroneRewardDataList = gd.DroneRewardDataList;

        DailyRandomDataList = gd.DailyRandomDataList;
        DailyStreakDataList = gd.DailyStreakDataList;

        BoosterData = new BoosterData() // nemt'om kell-e külön boosterData, de egye-fene
        {
            AutoTapBoosterDuration = gd.GeneralData.AutoTapBoosterDuration,
            AutoTapBoosterTapsPerSecond = gd.GeneralData.AutoTapBoosterTapsPerSecond,
            ExtraTimeBoosterBonus = gd.GeneralData.ExtraTimeBoosterBonus,
            TapStrengthBoosterDuration = gd.GeneralData.TapStrengthBoosterDuration,
            TapStrengthBoosterMultiplier = gd.GeneralData.TapStrengthBoosterMultiplier
        };
    }

    public override string GetFileName()
    {
        return GAME_DATA_PATH;
    }
    #endregion

}