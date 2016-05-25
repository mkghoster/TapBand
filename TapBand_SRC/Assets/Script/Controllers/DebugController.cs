using UnityEngine;
using System.Collections;

public class DebugController : MonoBehaviour {

    private SongController songController;
    private ConcertController concertController;
    private TourController tourController;
    private CurrencyController currencyController;

	
	void Start () {
        //songController = FindObjectOfType<SongController>();
        //concertController = FindObjectOfType<ConcertController>();
        //tourController = FindObjectOfType<TourController>();
        currencyController = FindObjectOfType<CurrencyController>();
    }
	
	// Update is called once per frame
	void Update () {
	
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

    }

}
