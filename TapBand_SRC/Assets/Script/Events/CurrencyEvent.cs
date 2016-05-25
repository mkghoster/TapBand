using System;

public delegate void CurrencyEvent(object sender, CurrencyEventArgs e);

public class CurrencyEventArgs : EventArgs
{
    public CurrencyEventArgs(double coin, double fan, int token)
    {
        Coin = coin;
        Fan = fan;
        Token = token;
    }

    public double Coin { get; private set; }
    public double Fan { get; private set; }
    public int Token { get; private set; }
}
