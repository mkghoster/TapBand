using UnityEngine;
using System.Collections.Generic;

public enum CharacterType
{
    Guitar1 = 0,
    Guitar2 = 1,
    Bass = 2,
    Keyboards = 3,
    Drums = 4
}

[System.Serializable]
public class CharacterData
{
    public int id;
    public string name;
    public double upgradeCost;
    public float tapStrengthBonus;
    public float merchBoothBonus;
    public float fanGainBonus;
    public float boosterTimeBonus;
    public float spotlightBonus;
    public float songIncomeBonus;
}
