using System;

public delegate void SettingsEvent(object sender, SettingsEventArgs e);

public class SettingsEventArgs : EventArgs
{
    public bool Sfx { get; private set; }
    public bool Music { get; private set; }

    public SettingsEventArgs(bool music, bool sfx)
    {
        Music = music;
        Sfx = sfx;
    }

}