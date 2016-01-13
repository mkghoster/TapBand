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
        float tapValue = CalculateTapValue();

        foreach(Vector3 position in args.positions)
        {
            tapUI.DisplayTapValueAt(position, (ulong) tapValue);
        }
        if (OnTap != null)
        {
            OnTap(tapValue * args.positions.Count);
        }
    }

    private float CalculateTapValue()
    {
        GameState state = GameState.instance;

        float equipmentMultiplier = 1;
        if (state.Equipment.CurrentBassEquipment != null)
        {
            equipmentMultiplier *= state.Equipment.CurrentBassEquipment.tapMultiplier;
        }
        if (state.Equipment.CurrentDrumEquipment != null)
        {
            equipmentMultiplier *= state.Equipment.CurrentDrumEquipment.tapMultiplier;
        }
        if (state.Equipment.CurrentGuitarEquipment != null)
        {
            equipmentMultiplier *= state.Equipment.CurrentGuitarEquipment.tapMultiplier;
        }
        if (state.Equipment.CurrentKeyboardEquipment != null)
        {
            equipmentMultiplier *= state.Equipment.CurrentKeyboardEquipment.tapMultiplier;
        }

        return equipmentMultiplier *
            DefaultOrSelf(state.Tour.CurrentTour.tapMultiplier);
    }

    private float DefaultOrSelf(float f)
    {
        return f == 0.0f ? 1.0f : f;
    }
}
