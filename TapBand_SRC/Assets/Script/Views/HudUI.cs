using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{

    public float heightOfBar;
    public float startingVerticalPos;

    private Text coinText;
    private Text fanText;
    private Text concertText;
    private Text songText;

    private SongData actualSongData;
    private float timePassed;
    private double progress;

    private double actualCoin;
    private double actualFan;
    private int actualToken;

    private double screenCoin;
    private double screenFan;
    private int screenToken;

    private CurrencyController currencyController;
    private SongController songController;

    void Awake()
    {
        currencyController = (CurrencyController)FindObjectOfType(typeof(CurrencyController));
        songController = (SongController)FindObjectOfType(typeof(SongController));

        var coinObj = GameObject.Find("CoinText");
        var fanObj = GameObject.Find("FanText");
        var concertObj = GameObject.Find("ConcertText");
        var songObj = GameObject.Find("SongText");

        coinText = coinObj.GetComponent<Text>();
        fanText = fanObj.GetComponent<Text>();
        concertText = concertObj.GetComponent<Text>();
        songText = songObj.GetComponent<Text>();
    }

    void OnEnable()
    {
        currencyController.OnInitialized += HandleCurrencyInitialized;
        currencyController.OnCurrencyChanged += HandleCurrencyEvent;

        songController.OnSongStarted += HandleSongStarted;
        songController.OnSongFinished += HandleSongFinished;
        songController.OnSongProgress += HandleSongProgress;
    }

    void OnDisable()
    {
        currencyController.OnInitialized -= HandleCurrencyInitialized;
        currencyController.OnCurrencyChanged += HandleCurrencyEvent;

        songController.OnSongStarted -= HandleSongStarted;
        songController.OnSongFinished -= HandleSongFinished;
        songController.OnSongProgress -= HandleSongProgress;
    }

    void OnGUI()
    {
        bool coinChanged = (screenCoin != actualCoin);
        bool fanChanged = (screenFan != actualFan);
        bool tokenChanged = (screenToken != actualToken);

        screenCoin = actualCoin;
        screenFan = actualFan;
        screenToken = actualToken;

        //TODO: do the pickup anim

        if (coinChanged)
        {
            coinText.text = screenCoin.ToString("F0");
        }
        if (fanChanged)
        {
            fanText.text = screenFan.ToString("F0");
        }
        if (tokenChanged)
        {
            // no such ui element
        }

        if (actualSongData != null)
        {
            GUI.color = Color.yellow;
            GUI.Box(
                new Rect(
                    Screen.width / 4 - 25f,
                    startingVerticalPos,
                    (float)(progress / actualSongData.tapGoal) * (Screen.width / 2) + 50f,
                    heightOfBar), string.Format("Taps: {0} / {1}", progress, actualSongData.tapGoal));

            GUI.color = Color.red;
            GUI.Box(
            new Rect(
                Screen.width / 4 - 25f,
                startingVerticalPos + heightOfBar + 5f,
                (timePassed * 5 / actualSongData.tapGoal) * (Screen.width / 2) + 50f,
                heightOfBar), string.Format("Time: {0} / {1}", timePassed, actualSongData.duration));

        }

    }

    private void HandleCurrencyEvent(object sender, CurrencyEventArgs e)
    {
        actualCoin = e.Coin;
        actualFan = e.Fan;
        actualToken = e.Token;
    }

    private void HandleCurrencyInitialized(object sender, CurrencyEventArgs e)
    {
        actualCoin = e.Coin;
        actualFan = e.Fan;
        actualToken = e.Token;

        screenCoin = e.Coin;
        screenFan = e.Fan;
        screenToken = e.Token;

        coinText.text = screenCoin.ToString("F0");
        fanText.text = screenFan.ToString("F0");
    }

    private void HandleSongProgress(object sender, SongEventArgs e)
    {
        timePassed = e.TimePassed;
        progress = e.Progress;
    }

    private void HandleSongStarted(object sender, SongEventArgs e)
    {
        actualSongData = e.Data;

        songText.text = "Song " + actualSongData.id.ToString();
        concertText.text = "Concert " + actualSongData.concertID;
    }

    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        actualSongData = null;
    }
}
