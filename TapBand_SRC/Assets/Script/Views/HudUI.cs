using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudUI : MonoBehaviour {

    public float heightOfBar;
    public float startingVerticalPos;

    public delegate string NewTourEvent();
    public event NewTourEvent NewTour;

    public delegate string NewCoinEvent();
    public event NewCoinEvent NewCoin;

    public delegate void NewFansEvent();
    public event NewFansEvent NewFans;

	public delegate string NewConcertEvent();
	public event NewConcertEvent NewConcert;

	public delegate string NewSongEvent();
	public event NewSongEvent NewSong;

    public delegate float ProcessEvent();
    public event ProcessEvent TimePassed;
    public event ProcessEvent TapPassed;

    public GameObject coin;
    public GameObject fan;
    public GameObject tour;
	public GameObject concert;
	public GameObject song;
    
    void Start () {
        coin = GameObject.Find("CoinText");
        fan = GameObject.Find("FanText");
        tour = GameObject.Find("TourText");
		concert = GameObject.Find("ConcertText");
		song = GameObject.Find("SongText");
    }
	
    void OnGUI()
    {
        if (NewTour != null) 
		{
            tour.GetComponent<Text>().text = "Tour: " + NewTour();
        }
        if (NewCoin != null)
        {
            coin.GetComponent<Text>().text = NewCoin();
        }
        if (NewFans != null)
        {
            NewFans();
        }
		if( NewConcert != null)
		{
			concert.GetComponent<Text>().text = NewConcert();
		}
		if( NewSong != null)
		{
			song.GetComponent<Text>().text = NewSong();
		}


        // TODO : finish for all the UI elements

        if (TapPassed != null)
        {
            GUI.color = Color.yellow;
            GUI.Box(
                new Rect(
                    Screen.width / 4,
                    startingVerticalPos,
                    TapPassed() * 5 + 50f,
                    heightOfBar), "Tap");
        }

        if (TimePassed != null)
        {
            GUI.color = Color.red;
            GUI.Box(
                new Rect(
                    Screen.width / 4,
                    startingVerticalPos + heightOfBar + 5f,
                    TimePassed() * 5 + 50f,
                    heightOfBar), "Time");
        }
    }
}
