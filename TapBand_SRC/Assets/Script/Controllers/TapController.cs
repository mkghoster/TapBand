using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour
{
    public float boosterMultiplier = 1f;
    public float boosterTimeInterval = 0f;

    public event TapEvent OnTap;

    #region Private fields
    private TapUI tapUI;

    private BandMemberController bandMemberController;
    private BoosterController boosterController;

    //private double prestigeTapMultiplier = 1f; 
    private float spotlightTapMultiplier;
    private double debugTapMultiplier;

    private CurrencyController currencyController;
    private ViewController viewController;
    #endregion

    void Awake()
    {
        BindWithUI();
        bandMemberController = FindObjectOfType<BandMemberController>();
        currencyController = FindObjectOfType<CurrencyController>();
        viewController = FindObjectOfType<ViewController>();
        boosterController = FindObjectOfType<BoosterController>();

        spotlightTapMultiplier = GameData.instance.GeneralData.SpotlightTapMultiplier;

        viewController.OnViewChange += ViewChanged;
    }

    void OnEnable()
    {
        tapUI.OnScreenTap += HandleTap;
        boosterController.OnAutoTap += HandleAutoTap;
    }

    void OnDisable()
    {
        tapUI.OnScreenTap -= HandleTap;
        boosterController.OnAutoTap -= HandleAutoTap;
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

        tapMultiplier *= boosterController.GetTapStrengthMultiplier();


        if (isSpotlight)
        {
            tapMultiplier *= spotlightTapMultiplier;
        }

        //tapMultiplier *= boosterMultiplier;
        //print("currencyController.TapMultiplierFromPrestige;: "+ currencyController.TapMultiplierFromPrestige);
        tapMultiplier *= currencyController.TapMultiplierFromPrestige;


        return tapMultiplier;
    }

    public void BoosterMultiplier(float multiplierValue)
    {
        boosterMultiplier = multiplierValue;
    }

    public void IncDebugTapMultiplier(double multiplier)
    {
        debugTapMultiplier *= multiplier;
        PlayerPrefsManager.SetDebugTapMultip((float)debugTapMultiplier);
    }

    public void ResetDebugTapMultiplier()
    {
        PlayerPrefsManager.SetDebugTapMultip(1.0f);
    }

    public void ViewChanged(object sender, ViewChangeEventArgs e)
    {
        if (e.NewView == ViewType.STAGE)
        {
            tapUI.ShowUI();
        }
        else
        {
            tapUI.HideUI();
        }
    }

    private void HandleAutoTap(object sender, RawTapEventArgs e)
    {
        tapUI.AutoTap(e);
    }
}
