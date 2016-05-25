using UnityEngine;
using System.Collections;
using System;

public delegate void RandomMechanismEvent(object sender, RandomMechanismEventArgs e);

public class RandomMechanismEventArgs : EventArgs
{
    public RandomMechanismType Type { get; private set; }
    
    public RandomMechanismEventArgs(RandomMechanismType type)
    {
        Type = type;
    }
}
