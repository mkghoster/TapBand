using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour
{
    private TapUI tapUI;
    private SongController songController;

    public float SpotlightTapMultiplier;
	public float boosterMultiplier=0f;
	public float boosterTimeInterval=0f;

    public delegate void TapEvent(float value);
    public event TapEvent OnTap;

    void Awake()
    {
        BindWithUI();

        songController = (SongController)FindObjectOfType(typeof(SongController));
    }

    void OnEnable()
    {
        tapUI.OnTap += HandleTap;
        songController.SwitchTapableCollider += SwitchTapAreaCollider;
    }

    void OnDisable()
    {
        tapUI.OnTap -= HandleTap;
        songController.SwitchTapableCollider -= SwitchTapAreaCollider;
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
			if (boosterMultiplier > 0 && boosterTimeInterval>0) {
				tapValue *= boosterMultiplier;
			}
            tapUI.DisplayTapValueAt(position, (ulong)tapValue, special);

            if (OnTap != null)
            {
                OnTap(tapValue);
            }
        }
    }

    private float CalculateTapValue(Vector2 position, bool special)
    {
        float tapMultiplier = GameState.instance.Currency.TapMultipliersProduct;
        // we should rename this parameter to isSpotlight, if there will be no other special cases
        if (special)
        {
            tapMultiplier *= DefaultOrSelf(SpotlightTapMultiplier);
        }
        return tapMultiplier;
    }

    private float DefaultOrSelf(float f)
    {
        return f == 0.0f ? 1.0f : f;
    }

	public void BoosterMultiplier(float multiplierValue){
		boosterMultiplier = multiplierValue;
	}

	public void BoosterTimeInterval(float multiplierIntervalValue){
		boosterTimeInterval = multiplierIntervalValue;
	}

    private void SwitchTapAreaCollider(bool value)
    {
        tapUI.SwitchOnOffCollider(value);
    }
}
