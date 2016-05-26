using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour
{
    private TapUI tapUI;
    private SongController songController;
    private BandMemberController bandMemberController;

    public float SpotlightTapMultiplier;
    public float boosterMultiplier = 1f;
    public float boosterTimeInterval = 0f;

    public event TapEvent OnTap;
    int i = 0;



    void Awake()
    {
        BindWithUI();
        songController = FindObjectOfType<SongController>();
        bandMemberController = FindObjectOfType<BandMemberController>();
    }

    void OnEnable()
    {
        tapUI.OnTap += HandleTap;
        songController.OnSongStarted += HandleSongStarted;
        songController.OnSongFinished += HandleSongFinished;
    }

    void OnDisable()
    {
        songController.OnSongStarted += HandleSongStarted;
        songController.OnSongFinished += HandleSongFinished;
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
            if (boosterMultiplier > 0 && boosterTimeInterval > 0)
            {
                tapValue *= boosterMultiplier;
            }
            tapUI.DisplayTapValueAt(position, (ulong)tapValue, special);

            if (OnTap != null)
            {
                OnTap(this, new TapEventArgs(tapValue));
            }
        }
    }

    private float CalculateTapValue(Vector2 position, bool special)
    {
        float tapMultiplier = 1;

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

    private void HandleSongStarted(object sender, SongEventArgs e)
    {
        tapUI.SwitchOnOffCollider(true);
    }

    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        tapUI.SwitchOnOffCollider(false);
    }
}
