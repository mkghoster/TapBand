using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    protected List<GameObject> sounds;
    protected int pooledAmount = 10;
    protected string hiearchyParentPath = "AudioManager/SFX";
    protected GameObject parentObject;
    public GameObject soundPrefab;
    public bool willGrow = true;



    void Start()
    {

    }

    void Update()
    {

    }

    protected void CreateSFXPool()
    {
        parentObject = GameObject.Find(hiearchyParentPath);
        sounds = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(soundPrefab);
            obj.transform.parent = parentObject.transform;
            obj.SetActive(false);
            sounds.Add(obj);
        }
    }

    protected void PlaySound()
    {
        for(int i=0;i<sounds.Count;i++)
        {
            if (!sounds[i].activeInHierarchy)
            {
                //PlaySound
                sounds[i].SetActive(true);
                return;
            }

        }
   
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(soundPrefab);
            obj.transform.parent = parentObject.transform;
            sounds.Add(obj);
            sounds[sounds.Count].SetActive(true);
        }
    }


}
