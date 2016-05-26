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

        //Debug info
        print("GameState path: "+ Application.persistentDataPath);

        LoadConnectionsInGameData();
        LoadDefaults();

        ConcertState concertState = GameState.instance.Concert;
        ConcertData currentConcert = gameData.ConcertDataList.FirstOrDefault(x => x.id == concertState.CurrentConcertID);
        concertState.CurrentSong = currentConcert.songList[concertState.CurrentSongIndex];
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
            GameState.instance.Concert.ResetToConcert(firstConcert);
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
        return GameData.instance.SongDataList.Where(x => x.concertID == concertID).ToList();
    }
}