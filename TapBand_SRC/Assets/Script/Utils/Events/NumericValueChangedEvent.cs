using UnityEngine;
using System.Collections;
using System;

public delegate void NumericValueChangedEventHandler(object sender, NumericValueChangedEventArgs e);

public class NumericValueChangedEventArgs : EventArgs
{
    public NumericValueChangedEventArgs(NumericValueType type, double delta)
    {
        Delta = delta;
    }

    public double Delta { get; private set; }
    public NumericValueType Type { get; private set; }
}

public enum NumericValueType
{
    FanAmount = 0,
    CoinAmount = 1,
    CurrentConcert = 2,
    CurrentTour = 3,
    SpotlightTaps = 4,
}