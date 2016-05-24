using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GeneralDataRecord
{
    public int id;
	public string name;
    public string value;
}

public class GeneralData
{
    #region PrivateFields
    private int merchBoothBoostPrice;
    private int merchBoothBoostUnitsInMinute;
    #endregion

    public int MerchBoothBoostPrice
    {
        get
        {
            return merchBoothBoostPrice;
        }
    }

    public int MerchBoothBoostUnitsInMinute
    {
        get
        {
            return merchBoothBoostUnitsInMinute;
        }
    }

    public void Init()
    {
        merchBoothBoostPrice = int.Parse(GameData.instance.FindGeneralDataByName("MerchBoothBoostPrice").value);
        merchBoothBoostUnitsInMinute = int.Parse(GameData.instance.FindGeneralDataByName("MerchBoothBoostUnitsInMinute").value);
    }
}
