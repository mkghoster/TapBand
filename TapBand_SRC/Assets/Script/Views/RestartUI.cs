using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void NewLevelEvent();

public class RestartUI : MonoBehaviour {

 
    public event NewLevelEvent NewLevel;
    
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
            Button btn = restartButton.GetComponent<Button>();
        }
    }
}
