using UnityEngine;
using System.Collections;
using System;

public delegate void BoosterEvent(object sender, BoosterEventArgs e);

public class BoosterEventArgs : EventArgs
{
    public BoosterType Type { get; private set; }

    public BoosterEventArgs(BoosterType type)
    {
        Type = type;
    }
}
