using UnityEngine;
using System.Collections.Generic;

public enum EquipmentType
{
    GUITAR, BASS, KEYBOARD, DRUM
}

[System.Serializable]
public class EquipmentData
{
	public int id;
	public EquipmentType equipmentType;
    public int level;
    public string name;
    public int upgradeCost;
    public float tapMultiplier;
}
