using UnityEngine;
using System.Collections;

/*
This class is purely for TEST purposes, and it gives some music at least...
*/
public class SongLoadController : MonoBehaviour {
    
    private AudioSource[] sounds;
    private int indexToPlay = 0;

	void Start () {
        sounds = GetComponents<AudioSource>();
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
}
