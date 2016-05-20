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

    void Awake()
    {
        currencyController = (CurrencyController)FindObjectOfType(typeof(CurrencyController));

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
    }

    void OnDisable()
    {
        currencyController.OnInitialized -= HandleCurrencyInitialized;
        currencyController.OnCurrencyChanged += HandleCurrencyEvent;
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

        //if (TapPassed != null)
        //{
        GUI.color = Color.yellow;
        //    if (TapPassed() >= (float)actualSongData.tapGoal && actualSongData.isEncore)
        //    {
        //        //print("vau");
        //        /*GUI.Box(
        //            new Rect(
        //                Screen.width / 4 - 25f,
        //                startingVerticalPos,
        //                (actualSongData.tapGoal) * (Screen.width / 2) + 50f,
        //                heightOfBar), "Tap");*/
        //        //print("TapGoal: "+actualSongData.tapGoal +  "  TapPassed: "+ TapPassed());
        //    }
        //    else
        GUI.Box(
            new Rect(
                Screen.width / 4 - 25f,
                startingVerticalPos,
                (float)(progress / actualSongData.tapGoal) * (Screen.width / 2) + 50f,
                heightOfBar), "Tap");
        //}

        //if (TimePassed != null)
        //{
        //    GUI.color = Color.red;
        //    if (actualSongData.isEncore)
        //    {
        //        GUI.Box(
        //        new Rect(
        //            Screen.width / 4 - 25f,
        //            startingVerticalPos + heightOfBar + 5f,
        //            (TimePassed() * 5 / actualSongData.tapGoal) * (Screen.width / 2) + 50f,
        //            heightOfBar), "Time");

        //        // print("bossbattle duration: " + actualSongData.duration);
        //        //print("bossbattle tapGoal: "  + actualSongData.tapGoal);
        //    }
        //    else  //************************** ÁTMENETI   ********************************************************* amíg a nem boss songok hossza 0 !!!!!!!!!!!!!!!!!!!!!!!!!!!
        //    {
        //        GUI.Box(
        //        new Rect(
        //           Screen.width / 4 - 25f,
        //           startingVerticalPos + heightOfBar + 5f,
        //           (TimePassed() * 5 / 80f) * (Screen.width / 2) + 50f,
        //           heightOfBar), "Time");
        //    }
        //    //print("timepassed: "+TimePassed() + " || tapgoal: "+ actualSongData.tapGoal);
        //}
    }

    private void HandleCurrencyEvent(object sender, CurrencyEventArgs e)
    {
        actualCoin = e.Coin;
        actualFan = e.Fan;
        actualToken = e.Token;
    }

    private void HandleCurrencyInitialized(object sender, CurrencyEventArgs e)
    {
        Debug.Log("HudUI: initialized currency controller");

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
}
