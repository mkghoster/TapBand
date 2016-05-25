using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnimController : MonoBehaviour {

    public event SongEvent OnSongFinished;
    public event TapEvent OnTap;
    private SongController songController;
    private Animator anim;
    private List<float> taps = new List<float>();
    public float tapsPerSecond;

    private TapUI tapUI;

    void Awake()
    {
        songController = FindObjectOfType<SongController>();
        tapUI = (TapUI)FindObjectOfType(typeof(TapUI));
        anim = GetComponent<Animator>();
    }

    void Start () {
        
    }

    void OnEnable()
    {
        songController.OnSongFinished += HandleSongFinished;
        tapUI.OnTap += CountTapsPerSecond;
    }

    private void CountTapsPerSecond(TapArgs args)
    {
        tapsPerSecond++;
        taps.Add(Time.timeSinceLevelLoad);
        // Debug.Log("szopdki");
    }

    void OnDisable()
    {
        songController.OnSongFinished -= HandleSongFinished;
        //tapUI.OnTap += CountTapsPerSecond;
    }

    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        if (e.Status == SongStatus.Successful)
        {
            bool isEncoreSong = e.Data.isEncore;
            if (isEncoreSong)
            {
                anim.SetTrigger("anim_palmy");
            }
            else
            {
                anim.SetTrigger("anim_rolling");
            }
            
        }
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended)
                taps.Add(Time.timeSinceLevelLoad);
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    taps.Add(Time.timeSinceLevelLoad);
        //}
        //Debug.Log("taps.Count " + taps.Count);
            for (int i = 0; i < taps.Count; i++)
        {
            if (taps[i] <= Time.timeSinceLevelLoad - 1)
            {
                taps.RemoveAt(i);
               // taps.Remove(i);
            }
        }
        tapsPerSecond = taps.Count;
        if (tapsPerSecond > 5)
        {
            anim.SetTrigger("anim_basic_02");
        }
        else if (tapsPerSecond > 3)
        {
            anim.SetTrigger("anim_basic_01");
        }
        else
        {
            anim.SetTrigger("anim_basic_03");
        }

        //Debug.Log(tapsPerSecond);
    }

}
