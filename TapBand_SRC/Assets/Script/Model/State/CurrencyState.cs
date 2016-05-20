using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class CurrencyState
{
    public double Fans { get; set; }
    public double Coins { get; set; }
    public int Tokens { get; set; }

    public IList<float> TapMultipliers { get; private set; }

    public CurrencyState()
    {
        TapMultipliers = new List<float>();
    }

    public float TapMultipliersProduct
    {
        get
        {
            float retVal = 1.0f;

            for (int i = 0; i < TapMultipliers.Count; i++)
            {
                retVal *= TapMultipliers[i];
            }

            return retVal;
        }
    }
}
