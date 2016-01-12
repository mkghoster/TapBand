using UnityEngine;
using System.Collections;

public abstract class BandMemberController : MonoBehaviour
{
    //private GameState gameState;

    void Start()
    {
        //gameState = GameState.instance;
    }

    public void TapEventOnBandMember()
    {
        // should notify, instead of this
        // catch the notification in HudController

        // gameState.increaseTapAndNumberOfFans();
        checkTapCountAndGiveMoney();
    }

    private void checkTapCountAndGiveMoney()
    {
        /*
        if (gameState.tapDuringSong > gameState.songLengthInSeconds)
        {
            gameState.money++;
        }
        */
    }
}
