using UnityEngine;
using System.Collections;

public class DebugController : MonoBehaviour
{
    #region Private fields
    private SongController songController;
    private ConcertController concertController;
    private TourController tourController;
    private CurrencyController currencyController;
    private TapController tapController;
    private DebugUI debugUI;
    private ViewController viewController;
    private StageController stageController;
    #endregion

    void Start()
    {
        //songController = FindObjectOfType<SongController>();
        //concertController = FindObjectOfType<ConcertController>();
        //tourController = FindObjectOfType<TourController>();
        currencyController = FindObjectOfType<CurrencyController>();
        tapController = FindObjectOfType<TapController>();
        viewController = FindObjectOfType<ViewController>();
        stageController = FindObjectOfType<StageController>();

        debugUI = FindObjectOfType<DebugUI>();
        debugUI.SetController(this);

        stageController.OnDebugButtonPressed += debugUI.ShowUI;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnEnable()
    {
        //currencyController.OnCurrencyChanged += AddCoin;
    }

    void OnDisable()
    {
    }

    //double-al error
    public void AddCoins(int coins)
    {
        currencyController.GiveCoins(coins);
    }

    public void AddTokens(int tokens)
    {
        currencyController.GiveTokens(tokens);
    }

    public void AddFans(int fans)
    {
        currencyController.GiveFans(fans);
    }

    public void IncTapStrength()
    {
        double tapMultipiler = 2.0;

        tapController.IncDebugTapMultiplier(tapMultipiler);
    }

    public void SetToOneDebugTapSterngth()//TODO ezt bekötni egy gombra !!!!!
    {
        tapController.SetToOneDebugTapMultiplier();
    }

    public void OnBackToGameClick()
    {
        viewController.EnterView(ViewType.BACKSTAGE);
    }
}
