using UnityEngine;
using System.Collections.Generic;

public enum BackgroundType
{
    BG_ALLEY = 0,
    BG_DARK_CITY = 1,
    BG_ICE = 2,
    BG_JAPAN = 3,
    BG_MAYA = 4,
    BG_POLAR_LIGHTS = 5,
    BG_SKYSCRAPER = 6
}


[System.Serializable]
public class ConcertData
{
    public int id;
    public string name;
    public int fanReward;
    public double rewardBase;
    public double levelRange;
    public BackgroundType background;

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
