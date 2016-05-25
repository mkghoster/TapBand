using UnityEngine;
using System.Collections.Generic;

public enum CharacterType
{
    Bass = 0,
    Drums = 1,
    Guitar1 = 2,
    Guitar2 = 3,
    Keyboards = 4,

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
