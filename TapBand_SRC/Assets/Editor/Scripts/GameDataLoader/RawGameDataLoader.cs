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

    private bool IsCellExist(int rowIndex, string columnName)
    {
        return rowIndex < currentRows.Count && currentRows[rowIndex].ContainsKey(columnName);
    }

    private void AbortWithErrorMessage(string msg)
    {
        EditorUtility.DisplayDialog("GameData Loading Error", msg, "Ok");
        throw new GameDataException(msg);
    }

    #region Type Specific Loaders

    // loads from consecutive columns
    private void TryLoadIntArray(int rowIndex, string baseColumnName, int columnNum, out int[] array)
    {
        array = new int[columnNum];

        for (int i = 0; i < columnNum; i++)
        {
            TryLoadInt(rowIndex, baseColumnName + i, out array[i]);
        }
    }

    private bool TryLoadInt(int rowIndex, string columnName, out int data)
    {
        if (!IsCellExist(rowIndex, columnName))
        {
            // AbortWithErrorMessage(currentSheet + "::" + columnName + " column is invalid! Row number: " + (rowIndex + 2));
            Debug.Log(currentSheet + "::" + columnName + " column is invalid! Row number: " + (rowIndex + 2));
            data = default(int);
            return false;
        }

        string[] splitCellValue = currentRows[rowIndex][columnName].Split('.'); // may contain number in format 10.0 etc

        if (!int.TryParse(splitCellValue[0], out data))
        {
            // AbortWithErrorMessage(currentSheet + "::" + columnName + " is not valid int! Row number: " + (rowIndex + 2));
            Debug.Log(currentSheet + "::" + columnName + " column is invalid! Row number: " + (rowIndex + 2));
            return false;
        }

        return true;
    }

    private void TryLoadFloat(int rowIndex, string columnName, out float data)
    {
        if (!IsCellExist(rowIndex, columnName))
        {
            AbortWithErrorMessage(currentSheet + "::" + columnName + " column is invalid! Row number: " + (rowIndex + 2));
        }

        string cellValue = currentRows[rowIndex][columnName];

        if (!float.TryParse(cellValue, out data))
        {
            AbortWithErrorMessage(currentSheet + "::" + columnName + " is not valid float! Row number: " + (rowIndex + 2));
        }
    }

    private void TryLoadBigInteger(int rowIndex, string columnName, out BigInteger data)
    {
        data = new BigInteger(0);
        try
        {
            string originalCellValue = "";
            try
            {
                originalCellValue = currentRows[rowIndex][columnName];
            }
            catch (Exception e)
            {
                AbortWithErrorMessage(currentSheet + "::" + columnName + " failed to parse as biginteger! Row number: " + (rowIndex + 1).ToString() + ". Error: " + e.Message);
            }
            if (originalCellValue.Contains("E"))
            {
                string cellValue = originalCellValue.Replace("E", ","); // to be easier to split

                if (cellValue.Contains("+"))
                {
                    cellValue = cellValue.Replace("+", "");
                }

                string[] splitValue = cellValue.Split(','); // {integerpart.fractionalpart , exponent}

                string[] baseNumStr = splitValue[0].Split('.');

                int exponent = int.Parse(splitValue[1]);
                BigInteger tempInt = new BigInteger(baseNumStr[0] + baseNumStr[1]);

                for (int i = 1; i <= (exponent - baseNumStr[1].Length); i++)
                {
                    tempInt *= 10;
                }

                data = tempInt;
            }
            else
            {
                string[] splitCellValue = originalCellValue.Split('.'); // may contain number in format 10.0 etc

                data = new BigInteger(splitCellValue[0]);
            }
        }
        catch (Exception e)
        {
            AbortWithErrorMessage(currentSheet + "::" + columnName + " failed to parse as biginteger! Row number: " + (rowIndex + 1).ToString() + ". Error: " + e.Message);
        }
    }

    private void TryLoadString(int rowIndex, string columnName, out string data)
    {
        if (!IsCellExist(rowIndex, columnName) || string.IsNullOrEmpty(currentRows[rowIndex][columnName]))
        {
            data = "";
            AbortWithErrorMessage(currentSheet + "::" + columnName + " is empty or null! Row number: " + (rowIndex + 2));
        }
        else
        {
            data = currentRows[rowIndex][columnName];
        }
    }

    private void TryLoadBool(int rowIndex, string columnName, out bool data)
    {
        if (!IsCellExist(rowIndex, columnName) || string.IsNullOrEmpty(currentRows[rowIndex][columnName]))
        {
            data = false;
            AbortWithErrorMessage(currentSheet + "::" + columnName + " is empty or null! Row number: " + (rowIndex + 2));
        }
        else
        {
            data = currentRows[rowIndex][columnName].Equals("TRUE") ? true : false;
        }
    }

    private bool TryLoadDouble(int rowIndex, string columnName, out double data)
    {
        if (!IsCellExist(rowIndex, columnName))
        {
            Debug.Log(currentSheet + "::" + columnName + " column is invalid! Row number: " + (rowIndex + 2));
            data = default(double);
            return false;
        }

        string cellValue = currentRows[rowIndex][columnName];

        if (!Double.TryParse(cellValue, out data))
        {
            Debug.Log(currentSheet + "::" + columnName + " is not valid double! Row number: " + (rowIndex + 2));
            return false;
        }
        return true;
    }

    private void TryLoadEnum<TEnum>(int rowIndex, string columnName, out TEnum data) where TEnum : struct, IConvertible
    {
        if (!IsCellExist(rowIndex, columnName) || string.IsNullOrEmpty(currentRows[rowIndex][columnName]))
        {
            data = default(TEnum);
            AbortWithErrorMessage(currentSheet + "::" + columnName + " is empty or null! Row number: " + (rowIndex + 2));
        }
        else
        {
            string enumString = currentRows[rowIndex][columnName];
            data = GetEnumFromString<TEnum>(enumString);
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

    public GameData LoadGameData()
    {
        GameData gameData = new GameData();
        LoadSongData(gameData.SongDataList);
        LoadConcertData(gameData.ConcertDataList);
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

            success = success && TryLoadInt(i, "ID", out songDataObject.id);

            TryLoadString(i, "Title", out songDataObject.title);
            TryLoadInt(i, "TapGoal", out songDataObject.tapGoal);
            TryLoadInt(i, "Duration", out songDataObject.duration);
            TryLoadInt(i, "CoinReward", out songDataObject.coinReward);
            TryLoadBool(i, "BossBattle", out songDataObject.bossBattle);
            TryLoadInt(i, "ConcertID", out songDataObject.concertID);

            //TODO: mindenhol megcsinálni, vagy megírni rendesen a függvényeket nem tryra (esetleg végigpróbálja?)
            if (!success)
            {
                return;
            }

            songDataTarget.Add(songDataObject);
        }

        return;
    }

    private void LoadConcertData(IList<ConcertData> concertDataTarget)
    {
        concertDataTarget.Clear();

        currentSheet = "ConcertData";
        currentRows = dataReader.GetRows(currentSheet);


        int rowNum = currentRows.Count;
        bool success = true;
        for (int i = 0; i < rowNum; i++)
        {
            ConcertData concertDataObject = new ConcertData();

            success = success && TryLoadInt(i, "ID", out concertDataObject.id);
            TryLoadString(i, "Name", out concertDataObject.name);
            success = success && TryLoadInt(i, "FanReward", out concertDataObject.fanReward);
            success = success && TryLoadDouble(i, "RewardBase", out concertDataObject.rewardBase);
            success = success && TryLoadDouble(i, "LevelRange", out concertDataObject.levelRange);
            TryLoadString(i, "Background", out concertDataObject.background);

            if (!success) { return; }

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

        int rowNum = currentRows.Count;
        bool success = true;
        for (int i = 0; i < rowNum; i++)
        {
            CharacterData equipmentDataObject = new CharacterData();

            success = TryLoadInt(i, "ID", out equipmentDataObject.id);
            TryLoadString(i, "Name", out equipmentDataObject.name);
            success = TryLoadDouble(i, "UpgradeCost", out equipmentDataObject.upgradeCost);
            TryLoadFloat(i, "TapStrengthBonus", out equipmentDataObject.tapStrengthBonus);
            TryLoadFloat(i, "MerchBoothBonus", out equipmentDataObject.merchBoothBonus);
            TryLoadFloat(i, "FanGainBonus", out equipmentDataObject.fanGainBonus);
            TryLoadFloat(i, "BoosterTimeBonus", out equipmentDataObject.boosterTimeBonus);
            TryLoadFloat(i, "SpotlightBonus", out equipmentDataObject.spotlightBonus);
            TryLoadFloat(i, "SongIncomeBonus", out equipmentDataObject.songIncomeBonus);

            if (!success)
            {
                return;
            }

            skillDataTarget.Add(equipmentDataObject);
        }
    }

    private void LoadSkinData(IList<SkinData> skinDataTarget, string sheetName)
    {
        skinDataTarget.Clear();

        List<Dictionary<string, string>> rawData = dataReader.GetRows(sheetName);
        bool success = true;
        for (var i = 0; i < rawData.Count; i++)
        {
            SkinData skinDataObject = new SkinData();
            success = success && TryLoadInt(i, "ID", out skinDataObject.id);
            TryLoadString(i, "Name", out skinDataObject.name);
            TryLoadString(i, "Icon", out skinDataObject.icon);
            TryLoadString(i, "Asset", out skinDataObject.asset);
            TryLoadEnum(i, "Type", out skinDataObject.type);
            TryLoadFloat(i, "TapStrengthBonus", out skinDataObject.tapStrengthBonus);
            success = success && TryLoadInt(i, "TokenCost", out skinDataObject.tokenCost);

            if (!success)
            {
                return;
            }

            skinDataTarget.Add(skinDataObject);
        }
    }

    private void LoadGeneralData(GeneralData generalDataTarget)
    {
        currentRows = dataReader.GetRows("GeneralData");

        int rowNum = currentRows.Count;
        for (int i = 0; i < rowNum; i++)
        {
            string currentRowName = null;
            TryLoadString(i, "Name", out currentRowName);

            switch (currentRowName)
            {
                case "DebugMessages":
                    TryLoadBool(i, "Value", out generalDataTarget.DebugMessages);
                    break;
                case "SpotlightInterval":
                    TryLoadFloat(i, "Value", out generalDataTarget.SpotlightInterval);
                    break;
                case "SpotlightTapMultiplier":
                    TryLoadFloat(i, "Value", out generalDataTarget.SpotlightTapMultiplier);
                    break;
                case "DroneMaxInterval":
                    TryLoadFloat(i, "Value", out generalDataTarget.DroneMaxInterval);
                    break;
                case "DroneIdleTime":
                    TryLoadFloat(i, "Value", out generalDataTarget.DroneIdleTime);
                    break;
                case "DroneMaxTaps":
                    TryLoadInt(i, "Value", out generalDataTarget.DroneMaxTaps);
                    break;
                case "DroneCoinLossRatio":
                    TryLoadFloat(i, "Value", out generalDataTarget.DroneCoinLossRatio);
                    break;
                case "RandomMechanismMinDelay":
                    TryLoadFloat(i, "Value", out generalDataTarget.RandomMechanismMinDelay);
                    break;
                case "RandomMechanismMaxDelay":
                    TryLoadFloat(i, "Value", out generalDataTarget.RandomMechanismMaxDelay);
                    break;
                case "TapStrengthBoosterMultiplier":
                    TryLoadFloat(i, "Value", out generalDataTarget.TapStrengthBoosterMultiplier);
                    break;
                case "TapStrengthBoosterDuration":
                    TryLoadFloat(i, "Value", out generalDataTarget.TapStrengthBoosterDuration);
                    break;
                case "ExtraTimeBoosterBonus":
                    TryLoadFloat(i, "Value", out generalDataTarget.ExtraTimeBoosterBonus);
                    break;
                case "AutoTapBoosterTapsPerSecond":
                    TryLoadFloat(i, "Value", out generalDataTarget.AutoTapBoosterTapsPerSecond);
                    break;
                case "DailyRandomResetHour":
                    TryLoadInt(i, "Value", out generalDataTarget.DailyRandomResetHour);
                    break;
                case "DailyRandomAdMultiplier":
                    TryLoadFloat(i, "Value", out generalDataTarget.DailyRandomAdMultiplier);
                    break;
                case "MerchBoothBoostPrice":
                    TryLoadInt(i, "Value", out generalDataTarget.MerchBoothBoostPrice);
                    break;
                case "MerchBoothBoostUnitsInMinute":
                    TryLoadInt(i, "Value", out generalDataTarget.MerchBoothBoostUnitsInMinute);
                    break;
                default:
                    AbortWithErrorMessage("General data key problems"); // TODO: error handling of the whole block
                    break;
            }
        }
    }

    private void LoadDroneRewardData(IList<DroneRewardData> droneRewardDataTarget)
    {
        droneRewardDataTarget.Clear();

        List<Dictionary<string, string>> rawData = dataReader.GetRows("DroneRewardData");
        bool success = true;
        for (var i = 0; i < rawData.Count; i++)
        {
            var droneRewardDataObject = new DroneRewardData();
            success = success && TryLoadInt(i, "ID", out droneRewardDataObject.id);
            TryLoadString(i, "Name", out droneRewardDataObject.name);
            TryLoadString(i, "Asset", out droneRewardDataObject.asset);
            TryLoadFloat(i, "Possibility", out droneRewardDataObject.possibility);
            TryLoadFloat(i, "CoinMultiplier", out droneRewardDataObject.coinMultiplier);
            success = success && TryLoadInt(i, "TokenAmount", out droneRewardDataObject.tokenAmount);
            TryLoadFloat(i, "TapStrengthBoostMultiplier", out droneRewardDataObject.tapStrengthBoostMultiplier);
            TryLoadFloat(i, "AutoTapPerSecond", out droneRewardDataObject.autoTapPerSecond);
            TryLoadFloat(i, "BoostDuration", out droneRewardDataObject.boostDuration);
            TryLoadBool(i, "IsAdRequired", out droneRewardDataObject.isAdRequired);

            if (!success)
            {
                return;
            }

            droneRewardDataTarget.Add(droneRewardDataObject);
        }
    }

    private void LoadDailyRandomData(IList<DailyRandomData> dailyRandomDataTarget)
    {
        dailyRandomDataTarget.Clear();

        List<Dictionary<string, string>> rawData = dataReader.GetRows("DailyRandomData");
        bool success = true;
        for (var i = 0; i < rawData.Count; i++)
        {
            var dailyRandomDataObject = new DailyRandomData();
            success = success && TryLoadInt(i, "ID", out dailyRandomDataObject.id);
            TryLoadString(i, "Name", out dailyRandomDataObject.name);
            TryLoadString(i, "Asset", out dailyRandomDataObject.asset);
            TryLoadFloat(i, "Possibility", out dailyRandomDataObject.possibility);
            TryLoadFloat(i, "CoinMultiplier", out dailyRandomDataObject.coinMultiplier);
            TryLoadFloat(i, "BoosterDiscount", out dailyRandomDataObject.boosterDiscount);
            TryLoadFloat(i, "AdMultiplier", out dailyRandomDataObject.adMultiplier);

            if (!success)
            {
                return;
            }

            dailyRandomDataTarget.Add(dailyRandomDataObject);
        }
    }

    private void LoadDailyStreakData(IList<DailyStreakData> dailyStreakDataTarget)
    {
        dailyStreakDataTarget.Clear();

        List<Dictionary<string, string>> rawData = dataReader.GetRows("DailyStreakData");
        bool success = true;
        for (var i = 0; i < rawData.Count; i++)
        {
            var dailyStreakDataObject = new DailyStreakData();
            success = success && TryLoadInt(i, "ID", out dailyStreakDataObject.id);
            TryLoadString(i, "Name", out dailyStreakDataObject.name);
            TryLoadString(i, "Asset", out dailyStreakDataObject.asset);
            TryLoadFloat(i, "CoinMultiplier", out dailyStreakDataObject.coinMultiplier);
            success = success && TryLoadInt(i, "TokenAmount", out dailyStreakDataObject.tokenAmount);

            if (!success)
            {
                return;
            }

            dailyStreakDataTarget.Add(dailyStreakDataObject);
        }
    }
}
