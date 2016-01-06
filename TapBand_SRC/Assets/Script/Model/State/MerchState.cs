using UnityEngine;
using System.Collections;

[System.Serializable]
public class MerchState {

    private int timeMerchId;
    private int qualityMerchId;

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
