using UnityEngine;
using System.Collections;

public class DebugController : MonoBehaviour
{
    #region Private fields
    //private SongController songController;
    //private ConcertController concertController;
    //private TourController tourController;
    private CurrencyController currencyController;
    private TapController tapController;
    private DebugUI debugUI;
    private ViewController viewController;
    private BackstageController backstageController;

    private const int tokensToAdd = 10;
    private const int fansToAdd = 50;
    private const int coinsToAdd = 100;
    #endregion

    void Start()
    {
        //songController = FindObjectOfType<SongController>();
        //concertController = FindObjectOfType<ConcertController>();
        //tourController = FindObjectOfType<TourController>();
        currencyController = FindObjectOfType<CurrencyController>();
        tapController = FindObjectOfType<TapController>();
        viewController = FindObjectOfType<ViewController>();
        backstageController = FindObjectOfType<BackstageController>();

        debugUI = FindObjectOfType<DebugUI>();
        debugUI.SetController(this);

        backstageController.OnDebugButtonPressed += debugUI.ShowUI;
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
    public void AddCoins()
    {
        currencyController.GiveCoins(coinsToAdd);
    }

    public void AddTokens()
    {
        currencyController.GiveTokens(tokensToAdd);
    }

    public void AddFans()
    {
        currencyController.GiveFans(fansToAdd);
    }

    public void IncTapStrength()
    {
        double tapMultipiler = 2.0;

        tapController.IncDebugTapMultiplier(tapMultipiler);
    }

    public void ResetDebugTapMultiplier()
    {
        tapController.ResetDebugTapMultiplier();
    }

    public void OnBackToGameClick()
    {
        viewController.EnterView(ViewType.BACKSTAGE);
    }
}
