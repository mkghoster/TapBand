using System;

public enum SkinType
{
    DRESS = 0,
    INSTRUMENT = 1
}

[Serializable]
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
