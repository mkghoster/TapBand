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
}
