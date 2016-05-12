using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EncoreButtonUI : MonoBehaviour {

    private SongController songController;
    private Button encoreButton;
    private Image encoreButtonImage;
    private Text childText;

    public delegate void GiveEncoreButtonPressed();
    public event GiveEncoreButtonPressed GiveEncoreButtonPressedEvent;

	void Awake () {
        songController = (SongController)FindObjectOfType(typeof(SongController));

        encoreButton = gameObject.GetComponent<Button>();
        encoreButtonImage = gameObject.GetComponent<Image>();
        childText = gameObject.GetComponentInChildren<Text>();
        DeactivateEncoreButton();
    }
	
	void Update () {
	    
	}

    public void GiveEncoreSong()
    {
        if(GiveEncoreButtonPressedEvent != null)
        {
            GiveEncoreButtonPressedEvent();
            DeactivateEncoreButton();
        }
    }

    void OnEnable()
    {
        songController.ShowEncoreButton += ActiveEncoreButton;
    }

    void OnDisable()
    {
        songController.ShowEncoreButton -= ActiveEncoreButton;
    }

    private void ActiveEncoreButton()
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
