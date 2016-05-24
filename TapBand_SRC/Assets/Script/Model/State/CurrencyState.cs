using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CurrencyState
{
    private int fans;
    [System.NonSerialized]
    private int screenFans;

    private double coins;
    [System.NonSerialized]
    private double screenCoins;

    private double tokens;
    [System.NonSerialized]
    private double screenTokens;

    private ICollection<Single> tapMultipliers = new List<Single>();

    public void AddTapMultiplier(float multiplier)
    {
        tapMultipliers.Add(multiplier);
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

    public void SynchronizeRealCurrencyAndScreenCurrency()
    {
        screenFans = fans;
        screenCoins = coins;
        screenTokens = tokens;
    }

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

    public double Coins
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

    public double Tokens
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
    
    public int ScreenFans
    {
        get
        {
            return screenFans;
        }

        set
        {
            screenFans = value;
        }
    }

    public double ScreenCoins
    {
        get
        {
            return screenCoins;
        }

        set
        {
            screenCoins = value;
        }
    }

    public double ScreenTokens
    {
        get
        {
            return screenTokens;
        }

        set
        {
            screenTokens = value;
        }
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

}
