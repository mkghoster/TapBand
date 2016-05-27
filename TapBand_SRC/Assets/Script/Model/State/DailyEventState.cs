using UnityEngine;
using System;

[Serializable]
public class DailyEventState
{
    public DateTime lastEventCompleted = DateTime.MinValue;
    public int lastStreakReward;
    public DateTime autoTapBoosterDiscountUntil = DateTime.MinValue;
    public DateTime extraTimeBoosterDiscountUntil = DateTime.MinValue;
    public DateTime tapStrengthBoosterDiscountUntil = DateTime.MinValue;
    public float autoTapBoosterDiscount;
    public float extraTimeBoosterDiscount;
    public float tapStrengthBoosterDiscount;

    public float AutoTapBoosterPriceMultiplier
    {
        get
        {
            return autoTapBoosterDiscountUntil > DateTime.UtcNow ? autoTapBoosterDiscount : 1;
        }
    }

    public float ExtraTimeBoosterPriceMultiplier
    {
        get
        {
            return extraTimeBoosterDiscountUntil > DateTime.UtcNow ? extraTimeBoosterDiscount : 1;
        }
    }

    public float TapStrengthBoosterPriceMultiplier
    {
        get
        {
            return tapStrengthBoosterDiscountUntil > DateTime.UtcNow ? tapStrengthBoosterDiscount : 1;
        }
    }
}
