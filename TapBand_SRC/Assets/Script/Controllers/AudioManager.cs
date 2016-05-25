using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using QuickPool;

using System;

public class AudioManager : MonoBehaviour {

     public Pool audioPool = new Pool()
     {
         size = 10,
         allowGrowth = true
     };


    protected SettingsUI settingsUI;
    protected float musicVolume;
    protected float sfxVolume;

    protected float fadeDuration = 1.0f;
    protected enum FadeState { FadeIn, FadeOut };

    private static  AudioManager instance;
    public  static  AudioManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AudioManager>();
            if (instance == null)
                instance = new GameObject("Audio Manager").AddComponent<AudioManager>();
      
            return instance;
        }
    }

    protected void InitAudioManager()
    {
        PoolsManager.RegisterPool(audioPool); //register pool if you want to use extention method Despawn
        audioPool.Initialize();

        settingsUI = GameObject.FindObjectOfType<SettingsUI>();

        //musicVolume = PlayerPrefsManager.GetMusicVolume();
        //sfxVolume = PlayerPrefsManager.GetSFXVolume();
        musicVolume = PlayerPrefs.GetFloat("music_volume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("sfx_volume", 0.5f);
    }



    
    public GameObject PlaySound(AudioClip clip, bool isLoop)
    {
        if (isLoop)
            return PlayLoopSound(clip);
        else
            PlayOneShotSound(clip);

        //if it's not a loop sound, then the reference doesn't matter
        return null;
            
    }

   

    //pool one sound
    private void PlayOneShotSound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero, sfxVolume);
    }

    
    //pooled loop sound, return the pooled object as a reference
    private GameObject PlayLoopSound(AudioClip clip)
    {      
        var spawn = audioPool.Spawn(Vector3.zero, Quaternion.identity);
        AudioSource spawnSource = spawn.GetComponent<AudioSource>();
        spawnSource.loop = true;
        spawnSource.clip = clip;  
        spawnSource.volume = sfxVolume;
        spawnSource.Play();

        return spawn.gameObject;
    }

    public void StopSound(GameObject targetToStop)
    {
        targetToStop.GetComponent<AudioSource>().Stop();
        targetToStop.GetComponent<AudioSource>().loop = false;
        targetToStop.GetComponent<AudioSource>().clip = null;

        audioPool.Despawn(targetToStop);
    }



    protected void FadeClip(AudioSource source, FadeState fadeState)
    {
        StartCoroutine(Fade(source, fadeState));   
    }

    private IEnumerator Fade(AudioSource source, FadeState fadeState)
    {
        if(fadeState == FadeState.FadeIn)
        {
            //source.volume = 0.0f;
            for (float f = 0f; f <= 1.0f; f += (Time.deltaTime / fadeDuration))
            {
                source.volume = Mathf.Lerp(0.0f, musicVolume, f);
                yield return null;
            }
            source.volume = musicVolume;    
        }
        else if(fadeState == FadeState.FadeOut)
        {  
            //source.volume = 1.0f;
            for (float f = 1f; f >= 0.0f; f -= (Time.deltaTime / fadeDuration))
            {
                source.volume = Mathf.Lerp(0.0f, musicVolume, f);   
                yield return null;
            }
            source.volume = 0.0f;   
        }
       
    }

    void OnEnable()
    {
        
    }

    //music changes handled in the child class
    void OnDisable()
    {
        audioPool.DespawnAll();
    }

    //called only in the child class
    protected void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }



}
