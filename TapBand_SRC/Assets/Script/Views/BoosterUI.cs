using UnityEngine;
using System.Collections;

public class BoosterUI : MonoBehaviour {

	private float TapStrengthBoosterMultiplier;
	private float TapStrengthBoosterDuration;
	private float CurrentTapStrengthBoosterDuration;
	private float ExtraTimeBoosterBonus;
	private float AutoTapBoosterInterval;
	private float AutoTapBoosterDuration;
	private float CurrentAutoTapBoosterDuration;
	private TapController tapController;
	private bool startCountDown=false;
	private bool autoTapStartCountDown=false;
	private SongController songController;

	public delegate void TapEvent(float value);
	public event TapEvent OnTap;
	private TapUI tapUI;
		
	void Start () {
		BoosterData ();
		tapController = (TapController)FindObjectOfType(typeof(TapController));
		songController = (SongController)FindObjectOfType(typeof(SongController));
		tapUI = (TapUI)FindObjectOfType(typeof(TapUI));
	}

	void Update () {
		if (startCountDown){
			CurrentTapStrengthBoosterDuration -= Time.deltaTime;
		}
		if (CurrentTapStrengthBoosterDuration <= 0) {
			startCountDown = false;
			tapController.BoosterTimeInterval (0);
		}

		if (autoTapStartCountDown){
			CurrentAutoTapBoosterDuration -= Time.deltaTime;
		}

		if (CurrentAutoTapBoosterDuration <= 0) {
			autoTapStartCountDown = false;
		}

//		if (autoTapStartCountDown) {
//			for (int i = 0; i < 10; i++) {
//				if (Time.time == i) {
//					
//				}
//			}
//				
//			
//		}
		if (CurrentAutoTapBoosterDuration > 0) {
	//		Debug.Log (CurrentAutoTapBoosterDuration);
		}
	}

    private void BoosterData()
    {
        foreach (GeneralData data in GameData.instance.GeneralDataList)
        {
            switch (data.name)
            {
                case "TapStrengthBoosterMultiplier":
                    float.TryParse(data.value, out TapStrengthBoosterMultiplier);
                    break;
                case "TapStrengthBoosterDuration":
                    float.TryParse(data.value, out TapStrengthBoosterDuration);
                    break;
                case "ExtraTimeBoosterBonus":
                    float.TryParse(data.value, out ExtraTimeBoosterBonus);
                    break;
                case "AutoTapBoosterInterval":
                    float.TryParse(data.value, out AutoTapBoosterInterval);
                    break;
                case "AutoTapBoosterDuration":
                    float.TryParse(data.value, out AutoTapBoosterDuration);
                    break;
                default:
                    break;
            }
        }
        Debug.Log("TapStrengthBoosterMultiplier " + TapStrengthBoosterMultiplier + "\n  TapStrengthBoosterDuration " + TapStrengthBoosterDuration + "\n  ExtraTimeBoosterBonus " + ExtraTimeBoosterBonus + "\n AutoTapBoosterInterval " + AutoTapBoosterInterval + "\n  AutoTapBoosterDuration " + AutoTapBoosterDuration);
    }

    public void HandleBoosters(string BoosterName){
		if (BoosterName.Equals ("TapStrengthBooster")) {
			tapController.BoosterMultiplier (TapStrengthBoosterMultiplier);
			tapController.BoosterTimeInterval (TapStrengthBoosterDuration);
			CurrentTapStrengthBoosterDuration = TapStrengthBoosterDuration;
			startCountDown = true;
		} else if (BoosterName.Equals ("AutoTapBooster")) {
			CurrentAutoTapBoosterDuration = AutoTapBoosterDuration;
			autoTapStartCountDown = true;

		} else if (BoosterName.Equals ("ExtraTimeBooster")) {
			songController.BossExtratime (ExtraTimeBoosterBonus);
		}
	}

}
