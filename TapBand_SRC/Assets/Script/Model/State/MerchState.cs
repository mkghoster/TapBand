using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class MerchState
{
    #region Private fields
    private int merchId; // = level
    private DateTime startTime;
    private MerchType merchType;

    //[System.NonSerialized]
    //private MerchData merchData;
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
            return GameState.instance.Concert.CurrentConcert.rewardBase * RewardMultiplier;
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
            return (int) Math.Round((float) SecsToFinish / 60.0f / (float) GameData.instance.GeneralDatas.MerchBoothBoostUnitsInMinute * (float) GameData.instance.GeneralDatas.MerchBoothBoostPrice);
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
            MerchData merch = GameData.instance.GetMerchDataByType(merchType).Find(c => c.id == merchId+1);
            return merch.cost;
        }
    }

    private MerchData MerchDataLink
    {
        get
        {
            if (merchId == 0)
            {
                return GameData.instance.GetMerchDataByType(merchType).Find(c => c.id == merchId + 1);
            }
            else
            {
                return GameData.instance.GetMerchDataByType(merchType).Find(c => c.id == merchId);
            }
        }
    }

    public void Init()
    {
        UpdateLinks();
    }

    public void UpdateLinks()
    {
        /*if (MerchId != 0)
        {
            merchData = GameData.instance.GetMerchDataByType(merchType).Find(c => c.id == merchId);
        }*/
    }

    public bool CanCollect()
    {
        return TokenToFinish <= GameState.instance.Currency.Tokens;
    }

    public void Collect()
    {
        if (!CanCollect())
        {
            return;
        }
        else
        {
            // TODO: Call currency controller to ask for Token
        }

        GameState.instance.Currency.Tokens -= TokenToFinish;
        GameState.instance.Currency.Coins += CollectibleCoins;
        startTime = DateTime.MaxValue;
    }

    public bool CanUpgrade()
    {
        //MerchData data = GameData.instance.GetMerchDataByType(merchType).Find(c => c.id == merchId+1);
        return UpgradeCost <= GameState.instance.Currency.Coins;
    }

    public void Upgrade()
    {
        if (!CanUpgrade())
        {
            return;
        }

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
