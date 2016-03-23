using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaySoundClip : MonoBehaviour {

    public AudioClip clip;

    private Button myButton;
    private AudioManager audioManager;
    public bool isLoop = false;

    public bool stopEvent = false;

	void Start () {
        myButton = gameObject.GetComponent<Button>();
        myButton.GetComponent<Button>().onClick.AddListener(() => { PlaySound(clip, isLoop);  });
        audioManager = AudioManager.Instance;
    }
	
    void Update()
    {
        if (stopEvent)
        {
            StopSound();
            stopEvent = false;
        }
    }

    private void PlaySound(AudioClip clip, bool isLoop)
    {
        audioManager.PlaySound(clip, gameObject.name, isLoop);
    }

    private void StopSound()
    {
        audioManager.StopSound(gameObject.name);
    }
}
