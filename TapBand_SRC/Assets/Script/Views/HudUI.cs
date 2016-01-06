using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudUI : MonoBehaviour {

    public float heightOfBar = 25f;
    public float startingVerticalPos = 60f;

    public delegate string NewTourEvent();
    public event NewTourEvent NewTour;

    public delegate void NewCoinEvent();
    public event NewCoinEvent NewCoin;

    public delegate void NewFansEvent();
    public event NewFansEvent NewFans;

	public delegate string NewConcertEvent();
	public event NewConcertEvent NewConcert;

	public delegate string NewSongEvent();
	public event NewSongEvent NewSong;

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
            NewCoin();
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

        GUI.color = Color.yellow;
        GUI.Box(
            new Rect(
                Screen.width / 4,
                startingVerticalPos,
                //GameState.instance.tapDuringSong * 5 + 50f,
                60f,
                heightOfBar), "Force");

        GUI.color = Color.red;
        GUI.Box(
            new Rect(
                Screen.width / 4,
                startingVerticalPos + heightOfBar + 5f,
                //GameState.instance.passedTimeInSeconds * 5 + 50f,
                60f,
                heightOfBar), "Time: ");

    }
}
