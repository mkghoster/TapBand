using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class AchievementState
{
    public AchievementState()
    {
        AchievementGroups = new List<AchievementGroup>();
    }

    public IList<AchievementGroup> AchievementGroups { get; private set; }

    public void UpsertAchievement(AchievementGroup AchievementGroup)
    {
        if (AchievementGroups.Contains(AchievementGroup))
        {
            //TODO: Update the achievement
        }
        else
        {
            //TODO: Insert the achievement
        }
    }
}
