using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaySoundClip : MonoBehaviour {

    public delegate void PlaySoundEffectEvent(AudioClip clip);
    public event PlaySoundEffectEvent PlaySoundEffect;


    public AudioClip clip;
    private Button myButton;
	void Start () {

        myButton = gameObject.GetComponent<Button>();
        myButton.GetComponent<Button>().onClick.AddListener(() => { PlaySound();  });
    }
	
	
    void PlaySound()
    {
        if (PlaySoundEffect != null)
        {
            PlaySoundEffect(clip);
        }
    }



}
