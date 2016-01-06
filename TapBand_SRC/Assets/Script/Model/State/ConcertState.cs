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

	
	//geci gány, proto jó lesz
	public SongData GetNextSong()
	{
		return GameData.instance.SongDataList.Find ( x => x.id == currentSong.id+1);
	}

	public ConcertData GetNextConcert()
	{
		return GameData.instance.ConcertDataList.Find ( x => x.id == CurrentConcert.id+1);
	}

	public SongData CurrentSong
	{
		get
		{
			return currentSong;
		}
		set
		{
			currentSong = value;
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
