using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class BoosterUI : MonoBehaviour
{
    public GameObject boosterPanel;

    public void SetUIVisible(bool visible)
    {
        boosterPanel.SetActive(visible);
    }
}
