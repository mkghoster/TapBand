using UnityEngine;
using System.Collections;
using System;

public delegate void TapEvent(object sender, TapEventArgs e);

public class TapEventArgs : EventArgs
{
    public TapEventArgs(float tapStrength)
    {
        TapStrength = tapStrength;
    }

    public float TapStrength { get; private set; }
}
