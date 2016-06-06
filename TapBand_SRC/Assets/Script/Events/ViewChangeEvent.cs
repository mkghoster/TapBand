using System;

public delegate void ViewChangeEvent(object sender, ViewChangeEventArgs e);

public class ViewChangeEventArgs : EventArgs
{
    public ViewChangeEventArgs(ViewType previousView, ViewType newView)
    {
        PreviousView = previousView;
        NewView = newView;
    }

    public ViewType PreviousView { get; private set; }
    public ViewType NewView { get; private set; }
}
