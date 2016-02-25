using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour
{
    private TapUI tapUI;

    public float SpotlightTapMultiplier;

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
        IterateOverPositions(args.positions, false);
        IterateOverPositions(args.spotlightPositions, true);
    }

    private void IterateOverPositions(ICollection<Vector2> positions, bool special)
    {
        foreach (Vector2 position in positions)
        {
            float tapValue = CalculateTapValue(position, special);
            tapUI.DisplayTapValueAt(position, (ulong)tapValue, special);

            if (OnTap != null)
            {
                OnTap(tapValue);
            }
        }
    }

    private float CalculateTapValue(Vector2 position, bool special)
    {
        GameState state = GameState.instance;

        float equipmentMultiplier = 1;

        if (special) // we should rename this parameter to isSpotlight, if there will be no other special cases
        {
            equipmentMultiplier *= SpotlightTapMultiplier;
        }

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
