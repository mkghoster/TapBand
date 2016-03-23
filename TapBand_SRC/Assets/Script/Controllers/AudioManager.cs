using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using QuickPool;

using System;

public class AudioManager : MonoBehaviour {

    private Dictionary<string, GameObject> loopSounds; 

     public Pool audioPool = new Pool()
     {
         size = 10,
         allowGrowth = true
     };


    protected SettingsUI settingsUI;
    protected float musicVolume;
    protected float sfxVolume;


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
        loopSounds = new Dictionary<string, GameObject>();

        PoolsManager.RegisterPool(audioPool); //register pool if you want to use extention method Despawn
        audioPool.Initialize();

        settingsUI = (SettingsUI)GameObject.FindObjectOfType(typeof(SettingsUI));

        musicVolume = PlayerPrefsManager.GetMusicVolume();
        sfxVolume = PlayerPrefsManager.GetSFXVolume();
    }



    public void StopSound(string name)
    {
        AudioSource spawnSource = loopSounds[name].GetComponent<AudioSource>();
        spawnSource.Stop();
        spawnSource.loop = false;
        spawnSource.clip = null;

        audioPool.Despawn(loopSounds[name]);

        loopSounds.Remove(name.ToString());
    }

    public void PlaySound(AudioClip clip, string name, bool isLoop)
    {
        if (isLoop)
            PlayLoopSound(clip, name);
        else
            PlayOneShotSound(clip);

    }

   

    //pool one sound
    private void PlayOneShotSound(AudioClip clip)
    {
        //print("PlayOneShotSound()");

        
        var spawn = audioPool.Spawn(Vector3.zero, Quaternion.identity);
        spawn.GetComponent<AudioSource>().PlayOneShot(clip, sfxVolume);

        StartCoroutine(WaitAndDespawn(clip.length, spawn));
    }

    private IEnumerator WaitAndDespawn(float waitTime, GameObject spawn)
    {
        yield return new WaitForSeconds(waitTime);
        audioPool.Despawn(spawn);
    }

    //pool loop sound
    private void PlayLoopSound(AudioClip clip, string name)
    {
        //print("PlayLoopSound()");
        
        var spawn = audioPool.Spawn(Vector3.zero, Quaternion.identity);
        AudioSource spawnSource = spawn.GetComponent<AudioSource>();
        spawnSource.loop = true;
        spawnSource.clip = clip;  
        spawnSource.volume = sfxVolume;
        spawnSource.Play();


        loopSounds.Add(name, spawn);

    }

    void OnEnable()
    {
        
    }

    //music changes handled in the child class
    void OnDisable()
    {
        audioPool.DespawnAll();
    }

    protected void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }



}
