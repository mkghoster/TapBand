using UnityEngine;
using System.Collections;

public class EquipmentController : MonoBehaviour {

    public delegate void ModifyCoinEvent(int price);
    public event ModifyCoinEvent CoinTransaction;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
