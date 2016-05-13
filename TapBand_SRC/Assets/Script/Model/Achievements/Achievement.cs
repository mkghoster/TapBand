using UnityEngine;
using System.Collections;

public abstract class Achievement
{
    //public abstract void CheckIsClaimable(GameStateEventArgs data);

    public void Claim()
    {
        if (IsClaimable && !IsClaimed)
        {
            //TODO: signal token gain
            IsClaimed = true;
        }
        //TODO: kell-e exception, ha nem claimelhet?
    }

    public bool IsClaimable { get; protected set; }
    public bool IsClaimed { get; protected set; }
}
