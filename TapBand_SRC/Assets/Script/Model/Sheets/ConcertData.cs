using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ConcertData
{
    public int id;
    public string name;
    public int fanReward;
    public double rewardBase;
    public double levelRange;
    public string background;

    public List<SongData> songList;

    public SongData GetEncoreSongData()
    {
        for (int i = 0; i < songList.Count; i++)
        {
            if (songList[i].isEncore)
            {
                return songList[i];
            }
        }
        return null; // Prepare for trouble!
    }
}
