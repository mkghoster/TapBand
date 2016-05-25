using UnityEngine;
using System.Collections;

public class BackgroundChange : MonoBehaviour {

    private ConcertController concertController;

    public GameObject[] backgrounds;

    private int numberOfBackgrounds;
    private SpriteRenderer spriteRenderer;

    private BackgroundType actualBackgroundID;

    void OnEnable()
    {
        concertController.OnConcertStarted += ChangeBackground;
    }

    void OnDisable()
    {
        concertController.OnConcertStarted -= ChangeBackground;
    }
	
	void Awake () {
        concertController = FindObjectOfType<ConcertController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
	
    void Start()
    {
        actualBackgroundID = concertController.CurrentConcertData.background;
        spriteRenderer.sprite = backgrounds[(int)actualBackgroundID].GetComponent<SpriteRenderer>().sprite;
    }

    private void ChangeBackground(object sender, ConcertEventArgs e)
    {
        actualBackgroundID = e.Data.background;
        spriteRenderer.sprite = backgrounds[(int)actualBackgroundID].GetComponent<SpriteRenderer>().sprite;
    }

}
