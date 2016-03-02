using UnityEngine;
using System.Collections;

/*
This class is purely for TEST purposes, and it gives some music at least...
*/
public class AudioPlayerController : MonoBehaviour {

    private ParticleSystem exp;
    private AudioSource[] sounds;
    private int indexToPlay = 0;

    private ConcertController concertController;

    void Awake()
    {
        concertController = (ConcertController)FindObjectOfType(typeof(ConcertController));
    }

    void OnEnable()
    {
        concertController.EndOfConcert += PlayTasty;
    }
    void OnDisable()
    {
        concertController.EndOfConcert -= PlayTasty;
    }

    void Start () {
        sounds = GetComponents<AudioSource>();
        exp = GetComponent<ParticleSystem>();
    }

    void Update () {
        if (GameState.instance.Concert != null && GameState.instance.Concert.CurrentSong != null)
        {
            int newIndexToPlay = (GameState.instance.Concert.CurrentSong.id + 1) % 2;
            if (indexToPlay != newIndexToPlay)
            {
                sounds[indexToPlay].Stop();
                sounds[newIndexToPlay].Play();

                indexToPlay = newIndexToPlay;
            }
        }
    }

    private void PlayTasty(ConcertData data)
    {
        sounds[2].Play();
        Explode();
    }

    void Explode()
    {
        exp.Play();
    }
}
