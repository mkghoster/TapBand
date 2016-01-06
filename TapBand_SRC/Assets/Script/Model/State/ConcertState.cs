using UnityEngine;
using System.Collections;

[System.Serializable]
public class ConcertState {

	private int currentConcertID;
	private int lastComplatedSongID;
	private SongData currentSong;
	
	
	public int CurrentConcertID
	{
		get
		{
			return currentConcertID;
		}
		set
		{
			currentConcertID = value;
		}
	}
	
	public int LastComplatedSongID
	{
		get
		{
			return lastComplatedSongID;
		}
		set
		{
			lastComplatedSongID = value;
		}
	}

	
	
	
	public ConcertData CurrentConcert
	{
		get
		{
			return GameData.instance.ConcertDataList.Find(x => x.id == currentConcertID);
		}
	}


}
