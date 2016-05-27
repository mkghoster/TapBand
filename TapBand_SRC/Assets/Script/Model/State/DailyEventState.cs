using UnityEngine;
using System;

[Serializable]
public class DailyEventState
{
    public DateTime lastEventCompleted = DateTime.MinValue;
    public int lastStreakReward;
}
