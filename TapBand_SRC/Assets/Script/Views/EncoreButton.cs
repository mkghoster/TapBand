using UnityEngine;
using System.Collections;

public class EncoreButton : MonoBehaviour {

    private SongController songController;

    public delegate void GiveEncoreButtonPressed();
    public event GiveEncoreButtonPressed GiveEncoreButtonPressedEvent;

	// Use this for initialization
	void Start () {
        songController = (SongController)FindObjectOfType(typeof(SongController));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GiveEncoreSong()
    {
        //give event
        //courtine megszakítása (a várakozás?)
        if(GiveEncoreButtonPressedEvent != null)
        {
            GiveEncoreButtonPressedEvent();
            print("11");
        }
        print("22");
    }
}
