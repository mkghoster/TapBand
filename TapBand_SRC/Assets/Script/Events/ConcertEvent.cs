using System;

public delegate void ConcertEvent(object sender, ConcertEventArgs e);


public class ConcertEventArgs : EventArgs
{

    public ConcertData Data { get; private set; }
    public ConcertState State { get; private set; }

    public ConcertEventArgs(ConcertData data, ConcertState state)
    {
        Data = data;
        State = state;
    }
}
