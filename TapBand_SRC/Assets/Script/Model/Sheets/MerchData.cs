using UnityEngine;
using System.Collections.Generic;

public enum MerchType
{
    TIME, QUALITY
}

[System.Serializable]
public class MerchData
{
	public int id;
	public MerchType merchType;
    public int level;
    public string name;
    public int upgradeCost;
    public int coinPerSecond;
    public int timeLimit;
}
