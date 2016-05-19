using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
    private EquipmentUI equipmentUI;

    void Awake()
    {
        equipmentUI = (EquipmentUI)FindObjectOfType(typeof(EquipmentUI));
    }

    private void ItemBoughtHandler(object sender, CharacterEventArgs e)
    {

    }
}
