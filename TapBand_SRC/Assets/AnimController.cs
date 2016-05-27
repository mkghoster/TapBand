using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnimController : MonoBehaviour {

    //public event SongEvent OnSongFinished;
    //public event TapEvent OnTap;
    private SongController songController;
    private Animator anim;
    private List<float> taps = new List<float>();
    private float tapsPerSecond;
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
        tapUI.OnScreenTap += CountTapsPerSecond;
    }

    private void CountTapsPerSecond(object sender, RawTapEventArgs args)
    {
        tapsPerSecond++;
        taps.Add(Time.timeSinceLevelLoad);
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
                anim.SetTrigger("palmy");
            }
            else
            {
                anim.SetTrigger("roll");
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
        //Debug.Log(asd);
        tapsPerSecond = taps.Count;
        if (tapsPerSecond > 5)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("lady_animation_basic_02"))
            {
                anim.SetTrigger("basic_02");
            }
            
        }
        else if (tapsPerSecond > 3)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("lady_animation_basic_01"))
            {
                anim.SetTrigger("basic_01");
            }
            
        }
        else
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("lady_animation_basic_03"))
            {
                anim.SetTrigger("basic_03");
            }
        }

        //Debug.Log(tapsPerSecond);
    }

}
