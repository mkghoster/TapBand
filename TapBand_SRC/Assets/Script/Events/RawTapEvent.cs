using System;
using System.Collections.Generic;

public delegate void RawTapEvent(object sender, RawTapEventArgs e);

public class RawTapEventArgs : EventArgs
{
    public RawTapEventArgs(IList<RawTapData> taps)
    {
        Taps = taps;
    }

    public IList<RawTapData> Taps // Ha igazán szépen akarjuk, akkor ez IEnumerable
    {
        get; private set;
    }
}
