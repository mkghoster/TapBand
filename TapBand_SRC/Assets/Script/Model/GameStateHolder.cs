using UnityEngine;
using System.Linq;
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
			cd.songList = GetAllSongsForConcert(cd.id);
		}

	}

	private List<SongData> GetAllSongsForConcert(int concertID)
	{
		return GameData.instance.SongDataList.Where(x => x.concertID == concertID).ToList(); // ez nem korrekt, a concert ismeri a songjait
	}

}