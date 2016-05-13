using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AchievementData
{
    public int Id;
    public string GroupCode;
    public string Title;
    public string ObjectiveFormatString;
    public int Rank;
    public double NumericGoal;
    public int TokenReward;
}
