using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class MerchState {

    private int timeMerchId;
    private int qualityMerchId;
    private DateTime lastOnlineDate;

    public int TimeMerchId
    {
        get
        {
            return timeMerchId;
        }

        set
        {
            timeMerchId = value;
        }
    }

    public int QualityMerchId
    {
        get
        {
            return qualityMerchId;
        }

        set
        {
            qualityMerchId = value;
        }
    }

    public DateTime LastOnlineDate
    {
        get
        {
            return lastOnlineDate;
        }

        set
        {
            lastOnlineDate = value;
        }
    }

    public MerchData CurrentQualityMerch
    {
        get
        {
            if (qualityMerchId == 0)
            {
                return null;
            }
            return GameData.instance.MerchDataList.Find(x => x.id == qualityMerchId);
        }
    }

    public MerchData CurrentTimeMerch
    {
        get
        {
            if (timeMerchId == 0)
            {
                return null;
            }
            return GameData.instance.MerchDataList.Find(x => x.id == timeMerchId);
        }
    }

}
