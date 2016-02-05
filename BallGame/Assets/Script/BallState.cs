using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BallState
{
    public int ballAliveTime;

    public int numberOfSuccessfulTaps;
    public int numberOfTaps; // not necessary

    public BallState(int bat)
    {
        this.ballAliveTime = bat;
    }
}
