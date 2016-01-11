using UnityEngine;
using System.Collections;

public class ConcertController : MonoBehaviour {

	private HudUI hud;
	private SongController songController;
    private TourController tourController;

    public delegate void RestartConcertEvent();
	public event RestartConcertEvent RestartConcert;

    public delegate void GiveEndOfConcertEvent(ConcertData concertData);
	public event GiveEndOfConcertEvent GiveEndOfConcert;

	void Awake()
	{
		hud = (HudUI)FindObjectOfType (typeof(HudUI));
        songController = (SongController)FindObjectOfType(typeof(SongController));
        tourController = (TourController)FindObjectOfType(typeof(TourController));
    }
	
	void OnEnable()
	{
		hud.NewConcert += DisplayNewConcert;
		songController.GiveNextSong += GiveNextSongFromConcert;
		songController.GiveFirstSongOfActualConcert += ResetToFirstSongOfConcert;
        tourController.RestartConcert += RestartConcertFromTheFirst;
    }
	void OnDisable()
	{
		hud.NewConcert -= DisplayNewConcert;
		songController.GiveNextSong -= GiveNextSongFromConcert;
		songController.GiveFirstSongOfActualConcert -= ResetToFirstSongOfConcert;
        tourController.RestartConcert -= RestartConcertFromTheFirst;
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
				if(GiveEndOfConcert != null)
				{
					GiveEndOfConcert(state.CurrentConcert);
				}
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
        if (RestartConcert != null)
        {
            RestartConcert();
        }
        //TODO: kikeresni az előző koncert utolsó számát
        state.CurrentSong = state.CurrentConcert.songList[0];
        state.LastComplatedSongID = state.CurrentSong.id;
        return state.CurrentSong;
	}
	
	private string DisplayNewConcert()
	{
		return GameState.instance.Concert.CurrentConcert.name;
	}

    private void RestartConcertFromTheFirst()
    {
        ConcertState state = GameState.instance.Concert;
        state.CurrentConcertID = GameData.instance.ConcertDataList[0].id;
        state.LastComplatedSongID = 0;
        state.CurrentSong = null;
    }
    
}
