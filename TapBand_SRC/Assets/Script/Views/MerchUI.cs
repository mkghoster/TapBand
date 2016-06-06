using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchUI : MonoBehaviour
{
    public GameObject MerchPanel;
    public GameObject MerchSlotPanel;
    public GameObject MerchScrollPanelContent;
    public GameObject MerchItemUIPrefab;
    public GameObject MerchSlotItemUIPrefab;

    #region Private fields
    private List<MerchItemUI> merchItems;
    private List<MerchSlotItemUI> merchSlotItems;
    MerchController merchController;
    #endregion

    void Awake()
    {
        merchItems = new List<MerchItemUI>();
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

    public void CreateMerchItems(IList<MerchState> merchStates)
    {
        MerchScrollPanelContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, merchStates.Count * 100 + 10);
        for (int i = 0; i < merchStates.Count; i++)
        {
            merchItems.Add(CreateMerchItem(merchStates[i]));
        }
    }

    private MerchItemUI CreateMerchItem(MerchState merchState)
    {
        GameObject gameObject = Instantiate<GameObject>(MerchItemUIPrefab);
        gameObject.transform.SetParent(MerchScrollPanelContent.transform, false);
        MerchItemUI itemUI = gameObject.GetComponent<MerchItemUI>();
        itemUI.SetupItem(this, merchState);
        return itemUI;
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

    public void UpdateMerchItems()
    {
        for (int i = 0; i < merchItems.Count; i++)
        {
            merchItems[i].UpdateItem();
        }
    }

    public void UpdateMerchSlotItems()
    {
        for (int i = 0; i < merchSlotItems.Count; i++)
        {
            merchSlotItems[i].UpdateItem();
        }
    }

    public void OnStartPressed(MerchState state)
    {
        merchController.OnStart(state);
    }

    public void OnCollectPressed(MerchState state)
    {
        merchController.OnCollect(state);
    }

    public void OnUpgradePressed(MerchState state)
    {
        merchController.OnUpgrade(state);
    }

    public void OnActivatePressed(MerchSlotState state)
    {
        merchController.OnActivate(state);
    }

    public bool HasFreeSlot()
    {
        return merchController.HasFreeSlot();
    }

    public void CloseButtonClick()
    {
        HideUI();
        merchController.OnClose();
    }

    public void HideUI()
    {
        MerchPanel.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        MerchPanel.gameObject.SetActive(true);
    }
}
