using UnityEngine;
using System.Collections;
using System;

public delegate void TapEvent(object sender, TapEventArgs e);

public class TapEventArgs : EventArgs
{
    public TapEventArgs(double tapStrength)
    {
        TapStrength = tapStrength;
    }

    public double TapStrength { get; private set; }
}