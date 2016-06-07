using UnityEngine;
using System.Collections;
using System;
using System.Linq;

[System.Serializable]
public class MerchState
{
    #region Private fields
    private int merchId; // = level
    private DateTime startTime;
    private MerchType merchType;

    [System.NonSerialized]
    private MerchData merchData;
    #endregion

    public MerchState(MerchType type)
    {
        merchId = 0;
        startTime = DateTime.MaxValue;
        merchType = type;
    }

    public int MerchId
    {
        get
        {
            return merchId;
        }
    }

    public bool Activated
    {
        get
        {
            return merchId != 0;
        }
    }

    public MerchType Type
    {
        get
        {
            return merchType;
        }
    }

    public double CollectibleCoins
    {
        get
        {
            return RewardMultiplier *
                GameData.instance.ConcertDataList.FirstOrDefault(c => c.id == GameState.instance.Concert.CurrentConcertID).rewardBase;
        }
    }

    public double CollectedCoins
    {
        get
        {
            return (Duration - SecsToFinish) / Duration * CollectibleCoins;
        }
    }

    public int TokenToFinish
    {
        get
        {
            return (int)Math.Ceiling((float)SecsToFinish / 60.0f / (float)GameData.instance.GeneralData.MerchBoothBoostUnitsInMinute * (float)GameData.instance.GeneralData.MerchBoothBoostPrice);
        }
    }

    public bool Started
    {
        get
        {
            return DateTime.UtcNow >= startTime;
        }
    }

    public int SecsToFinish
    {
        get
        {
            if (!Started)
            {
                return Duration;
            }
            return ((DateTime.UtcNow - startTime).TotalSeconds > (double)Duration) ? 0 : Duration - (int)(DateTime.UtcNow - startTime).TotalSeconds;
        }
    }

    public string Name
    {
        get
        {
            return MerchDataLink.name;
        }
    }

    public int Duration
    {
        get
        {
            return MerchDataLink.duration;
        }
    }

    public double RewardMultiplier
    {
        get
        {
            return MerchDataLink.rewardMultiplier;
        }
    }

    public double UpgradeCost
    {
        get
        {
            MerchData merch = GameData.instance.GetMerchDataByType(merchType).FirstOrDefault(c => c.id == merchId + 1);
            return merch.cost;
        }
    }

    private MerchData MerchDataLink
    {
        get
        {
            if (merchId == 0)
            {
                return GameData.instance.GetMerchDataByType(merchType).FirstOrDefault(c => c.id == merchId + 1);
            }
            else
            {
                return merchData;
            }
        }
    }

    public void Init()
    {
        UpdateLinks();
    }

    public void UpdateLinks()
    {
        if (MerchId != 0)
        {
            merchData = GameData.instance.GetMerchDataByType(merchType).FirstOrDefault(c => c.id == merchId);
        }
    }

    public bool CanCollect()
    {
        return TokenToFinish <= GameState.instance.Currency.Tokens;
    }

    public void ResetTimer()
    {
        startTime = DateTime.MaxValue;
    }

    public bool CanUpgrade()
    {
        return UpgradeCost <= GameState.instance.Currency.Coins;
    }

    public void Upgrade()
    {
        merchId += 1;
        UpdateLinks();
    }

    public void Start()
    {
        if (Started)
        {
            return;
        }
        startTime = DateTime.UtcNow;
    }
}
