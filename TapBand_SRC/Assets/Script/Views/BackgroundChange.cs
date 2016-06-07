using UnityEngine;
using System.Collections;

public class BackgroundChange : MonoBehaviour {

    private ConcertController concertController;
    private TourController tourController;

    public GameObject[] backgrounds;

    private int numberOfBackgrounds;
    private SpriteRenderer spriteRenderer;

    private BackgroundType actualBackgroundID;

    void OnEnable()
    {
        concertController.OnConcertStarted += ChangeBackground;
        tourController.OnPrestige += ResetBackground;
    }

    void OnDisable()
    {
        concertController.OnConcertStarted -= ChangeBackground;
        tourController.OnPrestige -= ResetBackground;
    }
	
	void Awake () {
        concertController = FindObjectOfType<ConcertController>();
        tourController = FindObjectOfType<TourController>();

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

    private void ResetBackground()
    {
        actualBackgroundID = 0;
        spriteRenderer.sprite = backgrounds[(int)actualBackgroundID].GetComponent<SpriteRenderer>().sprite;
    }

}
