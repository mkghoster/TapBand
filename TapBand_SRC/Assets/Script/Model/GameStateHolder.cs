using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateHolder : MonoBehaviour
{
    public GameData gameData;
    public GameState gameState;

    void Awake()
    {
        GameData.instance.TryLoadFromAssets(Application.streamingAssetsPath);
        GameState.instance.TryLoadFromAssets(Application.persistentDataPath);

        LoadDefaults();
		LoadConnectionsInGameData();
    }

    void OnDestroy()
    {
        GameState.instance.SaveToFile(Application.persistentDataPath);
    }

    private void LoadDefaults()
    {
        if (GameState.instance.Tour.CurrentTourIndex == 0)
        {
            TourData firstTour = GameData.instance.TourDataList[0];
            GameState.instance.Tour.CurrentTourIndex = firstTour.id;
        }

		if (GameState.instance.Concert.CurrentConcertID == 0) 
		{
			ConcertData firstConcert = GameData.instance.ConcertDataList[0];
			GameState.instance.Concert.CurrentConcertID = firstConcert.id;
		}


    }

	private void LoadConnectionsInGameData()
	{
		foreach (ConcertData cd in GameData.instance.ConcertDataList)
		{
			cd.songList = GetAllSongForConcert(cd.id);
		}

	}

	private List<SongData> GetAllSongForConcert(int concertID)
	{
		return GameData.instance.SongDataList.FindAll (x => x.concertID == concertID);
	}

}