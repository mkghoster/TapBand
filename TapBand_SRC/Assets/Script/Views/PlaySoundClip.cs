using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Collections.Generic;

public class PlaySoundClip : MonoBehaviour {

    //instance
    private AudioManager audioManager;

    //store the pooled element refernce and it's audioclip
    private Dictionary< AudioClip, GameObject> audioMap;

    void Start () {
        audioManager = AudioManager.Instance;
        audioMap = new Dictionary< AudioClip, GameObject>();
    }
	
    
    public void PlayLoopSound(AudioClip audioClip)
    {
        GameObject refToPoolMember = audioManager.PlaySound(audioClip, true);
        audioMap.Add(audioClip, refToPoolMember);
    }

    public void PlayOneShotSound(AudioClip audioClip)
    {
        audioManager.PlaySound(audioClip, false);
    }

    public void StopSound(AudioClip clip)
    {
        audioManager.StopSound( audioMap[clip] );
        audioMap.Remove(clip);
    }

  
}
