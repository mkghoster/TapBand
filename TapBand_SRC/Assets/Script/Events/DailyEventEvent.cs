using UnityEngine;
using System.Collections;
using System;

public delegate void DailyEventEvent(object sender, DailyEventEventArgs e);

public class DailyEventEventArgs : EventArgs
{
    public DailyEventEventArgs(DailyStreakData dailyStreakReward, DailyRandomData dailyRandomReward)
    {
        DailyStreakReward = dailyStreakReward;
    }

    public DailyStreakData DailyStreakReward { get; private set; }
    public DailyRandomData DailyRandomReward { get; private set; }
}
