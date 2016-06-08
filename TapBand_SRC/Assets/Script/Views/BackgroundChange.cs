using UnityEngine;
using System.Collections;

public class BackgroundChange : MonoBehaviour {

    #region private fields
    private ConcertController concertController;
    private TourController tourController;

    private GameObject[] backgrounds;
    private BackgroundType actualBackgroundID;

    private SpriteRenderer spriteRenderer;
    #endregion

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

        ReadBackgroundFromFolder();
    }
	
    void Start()
    {
        actualBackgroundID = concertController.CurrentConcertData.background;
        spriteRenderer.sprite = backgrounds[(int)actualBackgroundID].GetComponent<SpriteRenderer>().sprite;
    }

    private void ReadBackgroundFromFolder()
    {
        var array = Resources.LoadAll("Background", typeof(GameObject));
        backgrounds = new GameObject[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            backgrounds[i] = array[i] as GameObject;
        }
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
