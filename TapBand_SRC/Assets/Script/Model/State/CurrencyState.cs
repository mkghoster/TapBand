using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class CurrencyState
{
    private double tapMultiplierFromPrestige = 1.0;
    public double TapMultiplierFromPrestige
    {
        get
        {
            return tapMultiplierFromPrestige;
        }
        set
        {
            tapMultiplierFromPrestige = value;
        }
    }
    public double Fans { get; set; }
    public double Coins { get; set; }
    public int Tokens { get; set; }
}
