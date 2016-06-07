using System;

public delegate void SongEvent(object sender, SongEventArgs e);

public enum SongStatus
{
    InProgress = 0, // The song is in progress
    Successful = 1, // The song is completed succesfully
    Failed = 2,// The song has failed (eg.: the time ran out)
    EncoreInitiated = 3 // The encore button was pressed, the song has been interrupted (neither successful, neither failed)
}

public class SongEventArgs : EventArgs
{
    public SongData Data { get; private set; }
    public SongStatus Status { get; private set; }
    public double Progress { get; private set; }
    public float TimePassed { get; private set; }

    public SongEventArgs(SongData data, SongStatus status, double progress = 0, float timePassed = 0)
    {
        Data = data;
        Status = status;
        Progress = progress;
        TimePassed = timePassed;
    }
}
