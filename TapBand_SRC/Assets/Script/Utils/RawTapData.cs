using UnityEngine;
using System.Collections;

public struct RawTapData
{
    public RawTapData(Vector2 position, bool isSpotlight)
    {
        this.position = position;
        this.isSpotlight = isSpotlight;
    }

    public Vector2 position;
    public bool isSpotlight;
}
