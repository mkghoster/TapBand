using System;

public delegate void SongEvent(object sender, SongEventArgs e);

public enum SongStatus
{
    InProgress,
    Successful,
    Failed
}

public class SongEventArgs : EventArgs
{
    public SongData Data { get; private set; }
    public SongStatus Status { get; private set; }

    public SongEventArgs(SongData data, SongStatus status)
    {
        Data = data;
        Status = status;
    }
}
