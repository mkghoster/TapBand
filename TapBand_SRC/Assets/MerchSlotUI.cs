using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchSlotUI : MonoBehaviour
{
    public GameObject MerchSlotPanel;
    public GameObject MerchSlotItemUIPrefab;

    #region Private fields
    private List<MerchSlotItemUI> merchSlotItems;
    MerchController merchController;
    #endregion

    void Awake()
    {
        merchSlotItems = new List<MerchSlotItemUI>();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void SetController(MerchController controller)
    {
        merchController = controller;
    }

    public void CreateMerchSlotItems(IList<MerchSlotState> slotStates)
    {
        for (int i = 0; i < slotStates.Count; i++)
        {
            merchSlotItems.Add(CreateMerchSlotItem(slotStates[i]));
        }
    }

    private MerchSlotItemUI CreateMerchSlotItem(MerchSlotState slotState)
    {
        GameObject gameObject = Instantiate<GameObject>(MerchSlotItemUIPrefab);
        gameObject.transform.SetParent(MerchSlotPanel.transform, false);
        MerchSlotItemUI itemUI = gameObject.GetComponent<MerchSlotItemUI>();
        itemUI.SetupItem(this, slotState);
        return itemUI;
    }

    public void UpdateMerchSlotItems()
    {
        for (int i = 0; i < merchSlotItems.Count; i++)
        {
            merchSlotItems[i].UpdateItem();
        }
    }

    public void OnCollectPressed(MerchState state)
    {
        merchController.OnCollect(state);
    }

    public void OnActivatePressed(MerchSlotState state)
    {
        merchController.OnActivateSlot(state);
    }

    public void HideUI()
    {
        MerchSlotPanel.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        MerchSlotPanel.gameObject.SetActive(true);
    }
}
