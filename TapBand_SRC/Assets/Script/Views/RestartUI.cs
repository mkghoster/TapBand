using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RestartUI : MonoBehaviour {

    public delegate void NewLevelEvent();
    public event NewLevelEvent NewLevel;

    public delegate bool RestartEnabledEvent();
    public event RestartEnabledEvent RestartEnabled;

    public delegate TourData TourEvent();
    public event TourEvent CurrentTour;


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
            if (RestartEnabled != null)
            {
                btn.interactable = RestartEnabled();

                if (CurrentTour != null)
                {
                    TourData tour = CurrentTour();
                    btn.transform.Find("Text").GetComponent<Text>().text = "Restart (" + tour.fanRequirementToSkip + " fans)";
                }
            } else
            {
                restartButton.GetComponent<Button>().interactable = false;
            }
        }
    }
}
