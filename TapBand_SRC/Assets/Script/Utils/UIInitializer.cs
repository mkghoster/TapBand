using UnityEngine;
using System.Collections;

public class UIInitializer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // Turn off unnecessary UI elements
        transform.FindChild("SettingsUI").gameObject.SetActive(false);

        transform.FindChild("BackstageUI").gameObject.SetActive(false); ;

        var dressingRoomUI = transform.FindChild("DressingRoomUI");
        dressingRoomUI.FindChild("Drums").gameObject.SetActive(false);
        dressingRoomUI.FindChild("Guitar1").gameObject.SetActive(false);
        dressingRoomUI.FindChild("Guitar2").gameObject.SetActive(false);
        dressingRoomUI.FindChild("Keyboards").gameObject.SetActive(false);
        dressingRoomUI.gameObject.SetActive(false);
    }
}
