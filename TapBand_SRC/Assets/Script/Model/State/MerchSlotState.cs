using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public enum MerchSlotStatus
{
    CLOSED,
    EMPTY,
    ACTIVE,
    COMPLETE
}

[System.Serializable]
public class MerchSlotState
{
    #region Private fields
    private int merchSlotId;
    private bool activated;
    private MerchType activeMerchType;

    [NonSerialized]
    private MerchSlotData merchSlotData;
    #endregion

    public MerchSlotState(int id)
    {
        merchSlotId = id;
        activated = false;
        activeMerchType = MerchType.NONE;
    }

    public int MerchSlotId
    {
        get
        {
            return merchSlotId;
        }
    }

    public bool Activated
    {
        get
        {
            return activated;
        }
    }

    public MerchType ActiveMerchType
    {
        get
        {
            return activeMerchType;
        }
        set
        {
            activeMerchType = value;
        }
    }

    public MerchState ActiveMerchState
    {
        get
        {
            if (activeMerchType == MerchType.NONE)
            {
                throw new Exception("Wololo!");
            }
            return GameState.instance.MerchStates.FirstOrDefault(c => c.Type == activeMerchType);
        }
    }

    public double CoinCost
    {
        get
        {
            return merchSlotData.coinCost;
        }
    }

    public int TokenCost
    {
        get
        {
            return merchSlotData.tokenCost;
        }
    }

    public MerchSlotStatus Status
    {
        get
        {
            if (activated)
            {
                if (activeMerchType == MerchType.NONE)
                {
                    return MerchSlotStatus.EMPTY;
                }
                else
                {
                    if (ActiveMerchState.SecsToFinish == 0)
                    {
                        return MerchSlotStatus.COMPLETE;
                    }
                    else
                    {
                        return MerchSlotStatus.ACTIVE;
                    }
                }
            }
            else
            {
                return MerchSlotStatus.CLOSED;
            }
        }
    }

    public void Init()
    {
        UpdateLinks();
    }

    public void UpdateLinks()
    {
        if (merchSlotId == 0)
        {
            throw new Exception("Wololo!");
        }
        merchSlotData = GameData.instance.MerchSlotDataList.FirstOrDefault(c => c.id == merchSlotId);
    }

    public bool CanActivate()
    {
        if (CoinCost > 0)
        {
            return GameState.instance.Currency.Coins >= CoinCost;
        }
        else
        {
            return GameState.instance.Currency.Tokens >= TokenCost;
        }
    }

    public void Activate()
    {
        activated = true;
    }
}
