using UnityEngine;
using System.Collections;

[System.Serializable]
public class TourState {

    private int currentTourIndex;

    public int CurrentTourIndex
    {
        get
        {
            return currentTourIndex;
        }
        set
        {
            currentTourIndex = value;
        }
    }

    public TourData CurrentTour
    {
        get
        {
            return GameData.instance.TourDataList.Find(x => x.id == currentTourIndex);
        }
    }
}
