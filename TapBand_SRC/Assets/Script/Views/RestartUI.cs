using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RestartUI : MonoBehaviour {

    public delegate void NewLevelEvent();
    public event NewLevelEvent NewLevel;

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

            if (CurrentTour != null)
            {
                TourData tour = CurrentTour();
                btn.transform.Find("Text").GetComponent<Text>().text = "Restart with " + GameState.instance.Currency.NumberOfFans + " fans";
            }

        }
    }
}
