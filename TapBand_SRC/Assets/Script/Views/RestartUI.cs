using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RestartUI : MonoBehaviour {

    public delegate void NewLevelEvent();
    public event NewLevelEvent NewLevel;

    public delegate bool RestartEnabledEvent();
    public event RestartEnabledEvent RestartEnabled;

    public GameObject restartPanel, restartButton;
    
	public void RestartLevel()
	{
        if (NewLevel != null)
            NewLevel();
    }

    void OnGUI()
    {
        if (restartPanel.activeInHierarchy)
        {
            if (RestartEnabled != null)
            {
                restartButton.GetComponent<Button>().interactable = RestartEnabled();
            } else
            {
                restartButton.GetComponent<Button>().interactable = false;
            }
        }
    }
}
