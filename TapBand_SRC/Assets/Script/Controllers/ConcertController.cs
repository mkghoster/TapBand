using UnityEngine;
using System.Collections;

public class ConcertController : MonoBehaviour {

	private HudUI hud;
	private SongController songController;

	public delegate void RestartConcertEvent();
	public event RestartConcertEvent RestartConcert;
    
	void Awake()
	{
		hud = (HudUI)FindObjectOfType (typeof(HudUI));
        songController = (SongController)FindObjectOfType(typeof(SongController));
    }
	
	
	void OnEnable()
	{
		hud.NewConcert += DisplayNewConcert;
		songController.GiveNextSong += GiveNextSongFromConcert;
		songController.GiveFirstSongOfActualConcert += ResetToFirstSongOfConcert;

	}
	void OnDisable()
	{
		hud.NewConcert -= DisplayNewConcert;
		songController.GiveNextSong -= GiveNextSongFromConcert;
		songController.GiveFirstSongOfActualConcert -= ResetToFirstSongOfConcert;
	}

	//mi a kövtetkező lejátszandó szám, értesíti
	private SongData GiveNextSongFromConcert()
	{
		ConcertState state = GameState.instance.Concert;

		//ha boss battle
		if (state.CurrentSong != null) 
		{
            //új indítása
            if (state.CurrentSong.bossBattle)
            {
                state.CurrentConcertID = state.GetNextConcert().id;
            }

            state.LastComplatedSongID = state.CurrentSong.id;
        }
		state.CurrentSong = state.GetNextSong();

		return state.CurrentSong;
	}


	private SongData ResetToFirstSongOfConcert()
	{
		ConcertState state = GameState.instance.Concert;

		//TODO: kikeresni az előző koncert utolsó számát
		state.LastComplatedSongID = 0;

		state.CurrentSong = state.CurrentConcert.songList [0];

		return state.CurrentSong;
	}
	



	private string DisplayNewConcert()
	{
		return GameState.instance.Concert.CurrentConcert.name;
	}





}
