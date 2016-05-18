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

}
