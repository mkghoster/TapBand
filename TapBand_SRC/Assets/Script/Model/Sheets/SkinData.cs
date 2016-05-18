using UnityEngine;
using System.Collections;

public enum SkinType
{
    Dress = 0,
    Instrument = 1
}

public class SkinData
{
    public int id;
    public string name;
    public string icon;
    public string asset;
    public SkinType type;
    public float tapStrengthBonus;
    public int tokenCost;
}
