using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ConcertData
{
	public int id;
	public string name;
    public int fanReward;
    public string background;
    public double rewardBase;

	public List<SongData> songList; 

}
