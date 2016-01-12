using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MerchUI : MonoBehaviour {

    public delegate MerchData MerchDataEvent();
    public event MerchDataEvent CurrentQualityMerchData;
    public event MerchDataEvent NextQualityMerchData;
    public event MerchDataEvent CurrentTimeMerchData;
    public event MerchDataEvent NextTimeMerchData;

    public delegate void BuyMerchEvent(MerchData data);
    public event BuyMerchEvent BuyQualityMerch;
    public event BuyMerchEvent BuyTimeMerch;

    public delegate bool CanBuyEvent(int price);
    public event CanBuyEvent CanBuy;

    public GameObject timePanel, qualityPanel;

    void OnGUI()
    {
        RefreshPanel(timePanel, CurrentTimeMerchData, NextTimeMerchData, BuyTimeMerch);
        RefreshPanel(qualityPanel, CurrentQualityMerchData, NextQualityMerchData, BuyQualityMerch);
    }
    
    private void RefreshPanel(GameObject panel, MerchDataEvent currentData, MerchDataEvent nextData,
                              BuyMerchEvent buy)
    {
        if (currentData != null)
        {
            MerchData merchData = currentData();
            if (merchData != null)
            {
                GetTextComponentOfChild(panel, "CurrentLevelHolder").text = merchData.level.ToString();
                GetTextComponentOfChild(panel, "CurrentNameHolder").text = merchData.name;
                GetTextComponentOfChild(panel, "CurrentPropertiesHolder").text = MerchPropertiesToShow(merchData);
            }
        }

        GetButtonComponentOfChild(panel, "BuyButton").interactable = false;

        if (nextData != null)
        {
            MerchData nextMerchData = nextData();
            if (nextMerchData != null)
            {
                GetButtonTextComponentOfChild(panel, "BuyButton").text = "Buy " + nextMerchData.name + " (" + nextMerchData.upgradeCost + " coin)";
                GetTextComponentOfChild(panel, "NextMerchProperties").text = "It'll give " + MerchPropertiesToShow(nextMerchData);

                if (CanBuy != null)
                {
                    Button buyButton = GetButtonComponentOfChild(panel, "BuyButton");
                    buyButton.interactable = CanBuy(nextMerchData.upgradeCost);
                    // FIXME: temporary solution
                    buyButton.onClick.RemoveAllListeners();
                    buyButton.onClick.AddListener(() => buy(nextMerchData));
                }
            }

        }
    }

    private string MerchPropertiesToShow(MerchData merchData)
    {
        string ret = "";
        if (merchData.coinPerSecond != 0)
        {
            ret += merchData.coinPerSecond + " coin per sec ";
        }
        if (merchData.timeLimit != 0)
        {
            ret += merchData.timeLimit + " sec ";
        }
        return ret;
    }

    private Text GetTextComponentOfChild(GameObject parent, string childName)
    {
        return parent.transform.Find(childName).gameObject.GetComponent<Text>();
    }

    private Button GetButtonComponentOfChild(GameObject parent, string childName)
    {
        return parent.transform.Find(childName).gameObject.GetComponent<Button>();
    }

    private Text GetButtonTextComponentOfChild(GameObject parent, string childName)
    {
        return GetButtonComponentOfChild(parent, childName).transform.Find("Text").GetComponent<Text>();
    }
}
