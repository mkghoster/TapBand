using UnityEngine;
using System.Collections.Generic;

public enum MerchType
{
    NONE = 0,
    MERCH_1 = 1,
    MERCH_2,
    MERCH_3,
    MERCH_4,
    MERCH_5,
    MERCH_6,

    NUM_OF_MERCH_TYPES = MERCH_6
}

[System.Serializable]
public class MerchData
{
    public int id;
    public string name;
    public string icon;
    public double cost;
    public int duration;
    public double rewardMultiplier;
}
