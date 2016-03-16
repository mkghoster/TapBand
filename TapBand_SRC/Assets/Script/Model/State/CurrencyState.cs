using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CurrencyState
{
    private int fans;
    private int coins;
    private int tokens;
    private ICollection<Single> tapMultipliers = new List<Single>();

    public int Fans
    {
        get
        {
            return fans;
        }

        set
        {
            fans = value;
        }
    }

    public int Coins
    {
        get
        {
            return coins;
        }

        set
        {
            coins = value;
        }
    }

    public int Tokens
    {
        get
        {
            return tokens;
        }

        set
        {
            tokens = value;
        }
    }

    public void AddTapMultiplier(float multiplier)
    {
        tapMultipliers.Add(multiplier);
    }

    public ICollection<float> TapMultipliers
    {
        get
        {
            return tapMultipliers;
        }

        set
        {
            tapMultipliers = value;
        }
    }

    public float TapMultipliersProduct
    {
        get
        {
            float retVal = 1.0f;

            foreach (float f in tapMultipliers)
            {
                retVal *= f;
            }

            return retVal;
        }
    }
}
