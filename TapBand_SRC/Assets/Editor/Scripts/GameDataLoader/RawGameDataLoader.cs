using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excel;
using System;
using System.IO;
using System.Data;
using UnityEditor;

public class RawGameDataLoader : IGameDataLoader
{
    private string gameDataFilePath;
    private string currentSheet;
    private List<Dictionary<string, string>> currentRows;
    private NPOIExcelReader dataReader;

    public RawGameDataLoader(string _gameDataFilePath)
    {
        gameDataFilePath = _gameDataFilePath;
        currentSheet = "";
        dataReader = new NPOIExcelReader(gameDataFilePath);
    }

    public GameData LoadGameData()
    {
        GameData gameData = new GameData();

        LoadConcertData(gameData.ConcertDataList);
        LoadSongData(gameData.SongDataList);
        // gameData.MerchDataList = LoadMerchData();

        LoadCharacterData(gameData.CharacterData1List, "CharacterData1");
        LoadCharacterData(gameData.CharacterData2List, "CharacterData2");
        LoadCharacterData(gameData.CharacterData3List, "CharacterData3");
        LoadCharacterData(gameData.CharacterData4List, "CharacterData4");
        LoadCharacterData(gameData.CharacterData5List, "CharacterData5");

        LoadSkinData(gameData.SkinData1List, "SkinData1");
        LoadSkinData(gameData.SkinData2List, "SkinData2");
        LoadSkinData(gameData.SkinData3List, "SkinData3");
        LoadSkinData(gameData.SkinData4List, "SkinData4");
        LoadSkinData(gameData.SkinData5List, "SkinData5");

        //LoadMerchData(gameData.MerchData1List, "MerchData1");
        //LoadMerchData(gameData.MerchData2List, "MerchData2");
        //LoadMerchData(gameData.MerchData3List, "MerchData3");
        //LoadMerchData(gameData.MerchData4List, "MerchData4");
        //LoadMerchData(gameData.MerchData5List, "MerchData5");
        //LoadMerchData(gameData.MerchData6List, "MerchData6");

        //LoadMerchSlotData(gameData.MerchSlotDataList);

        LoadGeneralData(gameData.GeneralData);

        LoadDroneRewardData(gameData.DroneRewardDataList);

        LoadDailyRandomData(gameData.DailyRandomDataList);
        LoadDailyStreakData(gameData.DailyStreakDataList);

        return gameData;
    }

    private void LoadSongData(IList<SongData> songDataTarget)
    {
        songDataTarget.Clear();

        currentSheet = "SongData";
        currentRows = dataReader.GetRows(currentSheet);

        int rowNum = currentRows.Count;
        var success = true;
        for (int i = 0; i < rowNum; i++)
        {
            SongData songDataObject = new SongData();

            songDataObject.id = LoadInt(i, "ID");

            songDataObject.title = LoadString(i, "Title");
            songDataObject.tapGoal = LoadInt(i, "TapGoal");
            songDataObject.duration = LoadInt(i, "Duration");
            songDataObject.coinReward = LoadInt(i, "CoinReward");
            songDataObject.isEncore = LoadBool(i, "BossBattle");
            songDataObject.concertID = LoadInt(i, "ConcertID");

            //TODO: mindenhol megcsinálni, vagy megírni rendesen a függvényeket nem tryra (esetleg végigpróbálja?)
            if (!success)
            {
                Debug.Log("Failed to load songs");
                return;
            }

            songDataTarget.Add(songDataObject);
        }

        Debug.Log("Loaded " + songDataTarget.Count + " Songs");

        return;
    }

    private void LoadConcertData(IList<ConcertData> concertDataTarget)
    {
        concertDataTarget.Clear();

        currentSheet = "ConcertData";
        currentRows = dataReader.GetRows(currentSheet);

        int rowNum = currentRows.Count;

        for (int i = 0; i < rowNum; i++)
        {
            ConcertData concertDataObject = new ConcertData();

            concertDataObject.id = LoadInt(i, "ID");
            concertDataObject.name = LoadString(i, "Name");
            concertDataObject.fanReward = LoadInt(i, "FanReward");
            concertDataObject.rewardBase = LoadDouble(i, "RewardBase");
            concertDataObject.levelRange = LoadDouble(i, "LevelRange");
            concertDataObject.background = LoadEnum<BackgroundType>(i, "Background");

            concertDataTarget.Add(concertDataObject);
        }
    }

    //private List<MerchData> LoadMerchData()
    //{
    //    currentSheet = "MerchData";
    //    currentRows = dataReader.GetRows(currentSheet);

    //    List<MerchData> merchDataList = new List<MerchData>();

    //    int rowNum = currentRows.Count;
    //    for (int i = 0; i < rowNum; i++)
    //    {
    //        MerchData merchDataObject = new MerchData();

    //        TryLoadInt(i, "ID", out merchDataObject.id);
    //        TryLoadEnum(i, "MerchType", out merchDataObject.merchType);
    //        TryLoadInt(i, "Level", out merchDataObject.level);
    //        TryLoadString(i, "Name", out merchDataObject.name);
    //        TryLoadInt(i, "UpgradeCost", out merchDataObject.upgradeCost);
    //        TryLoadInt(i, "CoinPerSecond", out merchDataObject.coinPerSecond);
    //        TryLoadInt(i, "TimeLimit", out merchDataObject.timeLimit);

    //        merchDataList.Add(merchDataObject);
    //    }

    //    return merchDataList;
    //}

    private void LoadCharacterData(IList<CharacterData> skillDataTarget, string sheetName)
    {
        skillDataTarget.Clear();
        currentRows = dataReader.GetRows(sheetName);
        currentSheet = sheetName; //mert az exceptionkezelőnek azért kell...

        int rowNum = currentRows.Count;

        for (int i = 0; i < rowNum; i++)
        {
            CharacterData equipmentDataObject = new CharacterData();

            equipmentDataObject.id = LoadInt(i, "ID");
            equipmentDataObject.name = LoadString(i, "Name");
            equipmentDataObject.upgradeCost = LoadDouble(i, "UpgradeCost");
            equipmentDataObject.tapStrengthBonus = LoadFloat(i, "TapStrengthBonus");
            equipmentDataObject.merchBoothBonus = LoadFloat(i, "MerchBoothBonus");
            equipmentDataObject.fanGainBonus = LoadFloat(i, "FanGainBonus");
            equipmentDataObject.boosterTimeBonus = LoadFloat(i, "BoosterTimeBonus");
            equipmentDataObject.spotlightBonus = LoadFloat(i, "SpotlightBonus");
            equipmentDataObject.songIncomeBonus = LoadFloat(i, "SongIncomeBonus");

            skillDataTarget.Add(equipmentDataObject);
        }
    }

    private void LoadSkinData(IList<SkinData> skinDataTarget, string sheetName)
    {
        skinDataTarget.Clear();

        List<Dictionary<string, string>> rawData = dataReader.GetRows(sheetName);
        currentSheet = sheetName;
        currentRows = dataReader.GetRows(sheetName);

        for (var i = 0; i < rawData.Count; i++)
        {
            SkinData skinDataObject = new SkinData();

            skinDataObject.id = LoadInt(i, "ID");
            skinDataObject.name = LoadString(i, "Name");
            skinDataObject.icon = LoadString(i, "Icon");
            skinDataObject.asset = LoadString(i, "Asset");
            skinDataObject.type = LoadEnum<SkinType>(i, "Type");
            skinDataObject.tapStrengthBonus = LoadFloat(i, "TapStrengthBonus");
            skinDataObject.tokenCost = LoadInt(i, "TokenCost");

            skinDataTarget.Add(skinDataObject);
        }
    }

    private void LoadGeneralData(GeneralData generalDataTarget)
    {
        currentRows = dataReader.GetRows("GeneralData");
        currentSheet = "GeneralData";
        int rowNum = currentRows.Count;
        for (int i = 0; i < rowNum; i++)
        {
            string currentRowName = null;
            currentRowName = LoadString(i, "Name");

            switch (currentRowName)
            {
                case "DebugMessages":
                    generalDataTarget.DebugMessages = LoadBool(i, "Value");
                    break;
                case "SpotlightInterval":
                    generalDataTarget.SpotlightInterval = LoadFloat(i, "Value");
                    break;
                case "SpotlightTapMultiplier":
                    generalDataTarget.SpotlightTapMultiplier = LoadFloat(i, "Value");
                    break;
                case "DroneMaxInterval":
                    generalDataTarget.DroneMaxInterval = LoadFloat(i, "Value");
                    break;
                case "DroneIdleTime":
                    generalDataTarget.DroneIdleTime = LoadFloat(i, "Value");
                    break;
                case "DroneMaxTaps":
                    generalDataTarget.DroneMaxTaps = LoadInt(i, "Value");
                    break;
                case "DroneCoinLossRatio":
                    generalDataTarget.DroneCoinLossRatio = LoadFloat(i, "Value");
                    break;
                case "RandomMechanismMinDelay":
                    generalDataTarget.RandomMechanismMinDelay = LoadFloat(i, "Value");
                    break;
                case "RandomMechanismMaxDelay":
                    generalDataTarget.RandomMechanismMaxDelay = LoadFloat(i, "Value");
                    break;
                case "TapStrengthBoosterMultiplier":
                    generalDataTarget.TapStrengthBoosterMultiplier = LoadFloat(i, "Value");
                    break;
                case "TapStrengthBoosterDuration":
                    generalDataTarget.TapStrengthBoosterDuration = LoadFloat(i, "Value");
                    break;
                case "ExtraTimeBoosterBonus":
                    generalDataTarget.ExtraTimeBoosterBonus = LoadFloat(i, "Value");
                    break;
                case "AutoTapBoosterDuration":
                    generalDataTarget.AutoTapBoosterDuration = LoadFloat(i, "Value");
                    break;
                case "AutoTapBoosterTapsPerSecond":
                    generalDataTarget.AutoTapBoosterTapsPerSecond = LoadFloat(i, "Value");
                    break;
                case "DailyRandomResetHour":
                    generalDataTarget.DailyRandomResetHour = LoadInt(i, "Value");
                    break;
                case "DailyRandomAdMultiplier":
                    generalDataTarget.DailyRandomAdMultiplier = LoadFloat(i, "Value");
                    break;
                case "MerchBoothBoostPrice":
                    generalDataTarget.MerchBoothBoostPrice = LoadInt(i, "Value");
                    break;
                case "MerchBoothBoostUnitsInMinute":
                    generalDataTarget.MerchBoothBoostUnitsInMinute = LoadInt(i, "Value");
                    break;
                default:
                    break;
            }
        }
    }

    private void LoadDroneRewardData(IList<DroneRewardData> droneRewardDataTarget)
    {
        droneRewardDataTarget.Clear();
        currentSheet = "DroneRewardData";
        currentRows = dataReader.GetRows(currentSheet);
        List<Dictionary<string, string>> rawData = dataReader.GetRows("DroneRewardData");

        for (var i = 0; i < rawData.Count; i++)
        {
            var droneRewardDataObject = new DroneRewardData();
            droneRewardDataObject.id = LoadInt(i, "ID");
            droneRewardDataObject.name = LoadString(i, "Name");
            droneRewardDataObject.asset = LoadString(i, "Asset");
            droneRewardDataObject.possibility = LoadFloat(i, "Possibility");
            droneRewardDataObject.coinMultiplier = LoadFloat(i, "CoinMultiplier");
            droneRewardDataObject.tokenAmount = LoadInt(i, "TokenAmount");
            droneRewardDataObject.tapStrengthBoostMultiplier = LoadFloat(i, "TapStrengthBoostMultiplier");
            droneRewardDataObject.autoTapPerSecond = LoadFloat(i, "AutoTapPerSecond");
            droneRewardDataObject.boostDuration = LoadFloat(i, "BoostDuration");
            droneRewardDataObject.isAdRequired = LoadBool(i, "IsAdRequired");

            droneRewardDataTarget.Add(droneRewardDataObject);
        }
    }

    private void LoadDailyRandomData(IList<DailyRandomData> dailyRandomDataTarget)
    {
        dailyRandomDataTarget.Clear();
        currentSheet = "DailyRandomData";
        currentRows = dataReader.GetRows(currentSheet);
        List<Dictionary<string, string>> rawData = dataReader.GetRows("DailyRandomData");

        for (var i = 0; i < rawData.Count; i++)
        {
            var dailyRandomDataObject = new DailyRandomData();
            dailyRandomDataObject.id = LoadInt(i, "ID");
            dailyRandomDataObject.name = LoadString(i, "Name");
            dailyRandomDataObject.asset = LoadString(i, "Asset");
            dailyRandomDataObject.possibility = LoadFloat(i, "Possibility");
            dailyRandomDataObject.coinMultiplier = LoadFloat(i, "CoinMultiplier");
            dailyRandomDataObject.boosterDiscount = LoadFloat(i, "BoosterDiscount");
            dailyRandomDataObject.adMultiplier = LoadFloat(i, "AdMultiplier");

            dailyRandomDataTarget.Add(dailyRandomDataObject);
        }
    }

    private void LoadDailyStreakData(IList<DailyStreakData> dailyStreakDataTarget)
    {
        dailyStreakDataTarget.Clear();
        currentSheet = "DailyStreakData";
        currentRows = dataReader.GetRows(currentSheet);
        List<Dictionary<string, string>> rawData = dataReader.GetRows("DailyStreakData");

        for (var i = 0; i < rawData.Count; i++)
        {
            var dailyStreakDataObject = new DailyStreakData();
            dailyStreakDataObject.id = LoadInt(i, "ID");
            dailyStreakDataObject.name = LoadString(i, "Name");
            dailyStreakDataObject.asset = LoadString(i, "Asset");
            dailyStreakDataObject.coinMultiplier = LoadFloat(i, "CoinMultiplier");
            dailyStreakDataObject.tokenAmount = LoadInt(i, "TokenAmount");

            dailyStreakDataTarget.Add(dailyStreakDataObject);
        }
    }

    private bool IsCellExist(int rowIndex, string columnName)
    {
        return rowIndex < currentRows.Count && currentRows[rowIndex].ContainsKey(columnName);
    }

    #region Type Specific Loaders

    // loads from consecutive columns
    private void TryLoadIntArray(int rowIndex, string baseColumnName, int columnNum, out int[] array)
    {
        array = new int[columnNum];

        for (int i = 0; i < columnNum; i++)
        {
            array[i] = LoadInt(rowIndex, baseColumnName + i);
        }
    }

    private int LoadInt(int rowIndex, string columnName)
    {
        if (!IsCellExist(rowIndex, columnName))
        {
            throw new GameDataException(currentSheet + "::" + columnName + " column is invalid!Row number: " + (rowIndex + 2));
        }

        string[] splitCellValue = currentRows[rowIndex][columnName].Split('.'); // may contain number in format 10.0 etc

        int data;

        if (!int.TryParse(splitCellValue[0], out data))
        {
            throw new GameDataException(currentSheet + "::" + columnName + " column is invalid! Row number: " + (rowIndex + 2));
        }

        return data;
    }

    private float LoadFloat(int rowIndex, string columnName)
    {
        if (!IsCellExist(rowIndex, columnName))
        {
            throw new GameDataException(currentSheet + "::" + columnName + " column is invalid! Row number: " + (rowIndex + 2));
        }

        float data;
        if (!float.TryParse(currentRows[rowIndex][columnName], out data))
        {
            throw new GameDataException(currentSheet + "::" + columnName + " is not valid float! Row number: " + (rowIndex + 2));
        }

        return data;
    }

    private string LoadString(int rowIndex, string columnName)
    {
        if (!IsCellExist(rowIndex, columnName) || string.IsNullOrEmpty(currentRows[rowIndex][columnName]))
        {
            throw new GameDataException(currentSheet + "::" + columnName + " is empty or null! Row number: " + (rowIndex + 2));
        }
        return currentRows[rowIndex][columnName];
    }

    private bool LoadBool(int rowIndex, string columnName)
    {
        if (!IsCellExist(rowIndex, columnName) || string.IsNullOrEmpty(currentRows[rowIndex][columnName]))
        {
            throw new GameDataException(currentSheet + "::" + columnName + " is empty or null! Row number: " + (rowIndex + 2));
        }
        return currentRows[rowIndex][columnName].Equals("TRUE") ? true : false;
    }

    private double LoadDouble(int rowIndex, string columnName)
    {
        if (!IsCellExist(rowIndex, columnName))
        {
            throw new GameDataException(currentSheet + "::" + columnName + " column is invalid! Row number: " + (rowIndex + 2));
        }

        double data;

        if (!Double.TryParse(currentRows[rowIndex][columnName], out data))
        {
            throw new GameDataException(currentSheet + "::" + columnName + " is not valid double! Row number: " + (rowIndex + 2));
        }
        return data;
    }

    private TEnum LoadEnum<TEnum>(int rowIndex, string columnName) where TEnum : struct, IConvertible
    {
        if (!IsCellExist(rowIndex, columnName) || string.IsNullOrEmpty(currentRows[rowIndex][columnName]))
        {
            throw new GameDataException(currentSheet + "::" + columnName + " is empty or null! Row number: " + (rowIndex + 2));
        }
        else
        {
            string enumString = currentRows[rowIndex][columnName];
            return GetEnumFromString<TEnum>(enumString);
        }
    }

    public T GetEnumFromString<T>(string value) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        return (T)Enum.Parse(typeof(T), value);
    }


    #endregion
}