using UnityEngine;

public class TapController : MonoBehaviour
{
    private TapUI tapUI;

    public delegate void TapEvent(float value);
    public event TapEvent OnTap;

    void Awake()
    {
        BindWithUI();
    }

    void OnEnable()
    {
        tapUI.OnTap += HandleTap;
    }

    void OnDisable()
    {
        tapUI.OnTap -= HandleTap;
    }

    #region MVC bindings
    private void BindWithUI()
    {
        tapUI = (TapUI)FindObjectOfType(typeof(TapUI));
    }
    #endregion

    private void HandleTap(TapArgs args)
    {
        foreach(Vector3 position in args.positions)
        {
            tapUI.DisplayTapValueAt(position, 1);
        }
        if (OnTap != null)
        {
            float tapValue = CalculateTapValue(args.positions.Count);
            OnTap(tapValue);
        }
    }

    private float CalculateTapValue(int tapCount)
    {
        GameState state = GameState.instance;

        return tapCount *
            DefaultOrSelf(state.Tour.CurrentTour.tapMultiplier);
            //DefaultOrSelf(state.Equipment.CurrentHolyMageBladeOfJustice.tapMultiplier)
    }

    private float DefaultOrSelf(float f)
    {
        return f == 0.0f ? 1.0f : f;
    }
}
