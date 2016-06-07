using System.Collections;
using UnityEngine;

public enum ViewType
{
    STAGE = 1,
    BACKSTAGE = 2,
    CUSTOMIZATION = 3,
    DAILY_EVENT = 4
}

public class ViewController : MonoBehaviour
{
    public event ViewChangeEvent OnViewChange;

    #region Private fields
    private ViewType currentView;
    #endregion

    public ViewType CurrentView
    {
        get
        {
            return currentView;
        }
    }

    public ViewController()
    {
        currentView = ViewType.STAGE;
    }

    public void EnterView(ViewType newView)
    {
        if (OnViewChange != null)
        {
            OnViewChange(this, new ViewChangeEventArgs(currentView, newView));
        }
        currentView = newView;
    }
}
