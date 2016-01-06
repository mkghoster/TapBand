using UnityEngine;
using System.Collections;

public class ConcertController : MonoBehaviour {

	private HudUI hud;
	
	
	void Awake()
	{
		hud = (HudUI)FindObjectOfType (typeof(HudUI));
	}
	
	
	void OnEnable()
	{
		hud.NewConcert += DisplayNewConcert;
	}
	void OnDisable()
	{
		hud.NewConcert -= DisplayNewConcert;
	}
	

	
	private string DisplayNewConcert()
	{
		return GameState.instance.Concert.CurrentConcert.name;
	}
	

}
