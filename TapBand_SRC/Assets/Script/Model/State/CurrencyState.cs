using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CurrencyState
{
    private int fans;
    [System.NonSerialized]
    private int screenFans;

    private int coins;
    [System.NonSerialized]
    private int screenCoins;

    private int tokens;
    [System.NonSerialized]
    private int screenTokens;

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

    public int ScreenCoins
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

    public int ScreenTokens
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
