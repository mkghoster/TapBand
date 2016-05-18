using UnityEngine;
using System.Collections;
using System;

public abstract class Achievement
{
    public bool TrySetClaimable(double value)
    {
        if (value >= TargetValue)
        {
            IsClaimable = true;
        }
        return IsClaimable;
    }

    public bool TryClaim()
    {
        if (IsClaimable && !IsClaimed)
        {
            IsClaimed = true;
        }
        return IsClaimed;
    }

    public int Id { get; protected set; }

    [NonSerialized]
    private AchievementGroup achievementGroup;
    public AchievementGroup AchievementGroup
    {
        get
        { return achievementGroup; }
        protected set
        {
            achievementGroup = value;
        }
    }

    public bool IsClaimable { get; protected set; }
    public bool IsClaimed { get; protected set; }
    protected double TargetValue { get; set; }
}
