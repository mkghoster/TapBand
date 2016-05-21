using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EncoreButtonUI : MonoBehaviour
{

    private SongController songController;
    private ConcertController concertController;
    private Button encoreButton;
    private Image encoreButtonImage;
    private Text childText;

    public delegate void GiveEncoreButtonPressed();
    public event GiveEncoreButtonPressed GiveEncoreButtonPressedEvent;

    void Awake()
    {
        songController = FindObjectOfType<SongController>();
        concertController = FindObjectOfType<ConcertController>();

        encoreButton = gameObject.GetComponent<Button>();
        encoreButtonImage = gameObject.GetComponent<Image>();
        childText = gameObject.GetComponentInChildren<Text>();
        DeactivateEncoreButton();
    }

    void Update()
    {

    }

    public void GiveEncoreSong()
    {
        if (GiveEncoreButtonPressedEvent != null)
        {
            GiveEncoreButtonPressedEvent();
            DeactivateEncoreButton();
        }
    }

    void OnEnable()
    {
        songController.OnSongStarted += HandleSongStarted;
        songController.OnSongFinished += HandleSongFinished;
    }

    void OnDisable()
    {
        songController.OnSongStarted -= HandleSongStarted;
        songController.OnSongFinished -= HandleSongFinished;
    }

    private void HandleSongStarted(object sender, SongEventArgs e)
    {
        if (concertController.HasTriedEncore && concertController.IsNextSongEncore)
        {
            ActivateEncoreButton();
        }
    }

    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        DeactivateEncoreButton();
    }

    private void ActivateEncoreButton()
    {
        encoreButton.enabled = true;
        encoreButtonImage.enabled = true;
        childText.enabled = true;

        //  gameObject.SetActive(true);
    }

    private void DeactivateEncoreButton()
    {
        encoreButton.enabled = false;
        encoreButtonImage.enabled = false;
        childText.enabled = false;

        // gameObject.SetActive(false);
    }

}
