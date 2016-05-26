using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour
{
    private TapUI tapUI;

    private BandMemberController bandMemberController;

    private float spotlightTapMultiplier;
    public float boosterMultiplier = 1f;
    public float boosterTimeInterval = 0f;

    public event TapEvent OnTap;
    int i = 0;

    private double debugTapMultiplier;

    void Awake()
    {
        BindWithUI();
        bandMemberController = FindObjectOfType<BandMemberController>();

        spotlightTapMultiplier = GameData.instance.GeneralData.SpotlightTapMultiplier;
    }

    void OnEnable()
    {
        tapUI.OnScreenTap += HandleTap;
    }

    void OnDisable()
    {
        tapUI.OnScreenTap -= HandleTap;
    }

    #region MVC bindings
    private void BindWithUI()
    {
        tapUI = (TapUI)FindObjectOfType(typeof(TapUI));
    }
    #endregion

    private void HandleTap(object sender, RawTapEventArgs e)
    {
        for (int i = 0; i < e.Taps.Count; i++)
        {
            double tapValue = CalculateTapValue(e.Taps[i].position, e.Taps[i].isSpotlight);
            tapUI.DisplayTapValueAt(e.Taps[i], tapValue);

            if (OnTap != null)
            {
                OnTap(this, new TapEventArgs(tapValue));
            }
        }
    }

    private double CalculateTapValue(Vector2 position, bool isSpotlight)
    {
        //double tapMultiplier = 1;
        debugTapMultiplier = (double)PlayerPrefsManager.GetDebugTapMultip();
        double tapMultiplier = 1 * debugTapMultiplier;  //DEBUG

        for (int i = 0; i < bandMemberController.UnlockedUpgrades[CharacterType.Bass].Count; i++)
        {
            tapMultiplier *= bandMemberController.UnlockedUpgrades[CharacterType.Bass][i].tapStrengthBonus;
        }
        for (int i = 0; i < bandMemberController.UnlockedUpgrades[CharacterType.Drums].Count; i++)
        {
            tapMultiplier *= bandMemberController.UnlockedUpgrades[CharacterType.Drums][i].tapStrengthBonus;
        }
        for (int i = 0; i < bandMemberController.UnlockedUpgrades[CharacterType.Guitar1].Count; i++)
        {
            tapMultiplier *= bandMemberController.UnlockedUpgrades[CharacterType.Guitar1][i].tapStrengthBonus;
        }
        for (int i = 0; i < bandMemberController.UnlockedUpgrades[CharacterType.Guitar2].Count; i++)
        {
            tapMultiplier *= bandMemberController.UnlockedUpgrades[CharacterType.Guitar2][i].tapStrengthBonus;
        }
        for (int i = 0; i < bandMemberController.UnlockedUpgrades[CharacterType.Keyboards].Count; i++)
        {
            tapMultiplier *= bandMemberController.UnlockedUpgrades[CharacterType.Keyboards][i].tapStrengthBonus;
        }

        if (boosterMultiplier > 0 && boosterTimeInterval > 0)
        {
            tapMultiplier *= boosterMultiplier;
        }

        if (isSpotlight)
        {
            tapMultiplier *= spotlightTapMultiplier;
        }

        //tapMultiplier *= boosterMultiplier;

        return tapMultiplier;
    }

    public void BoosterMultiplier(float multiplierValue)
    {
        boosterMultiplier = multiplierValue;
    }

    public void BoosterTimeInterval(float multiplierIntervalValue)
    {
        boosterTimeInterval = multiplierIntervalValue;
    }

    public void IncDebugTapMultiplier(double multiplier)
    {
        debugTapMultiplier *= multiplier;
        PlayerPrefsManager.SetDebugTapMultip((float)debugTapMultiplier);
    }
}
