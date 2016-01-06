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
        if (CurrentQualityMerchData != null)
        {
            MerchData qualityData = CurrentQualityMerchData();
            if (qualityData != null)
            {
                GetTextComponentOfChild(qualityPanel, "CurrentLevelHolder").text = qualityData.level.ToString();
                GetTextComponentOfChild(qualityPanel, "CurrentNameHolder").text = qualityData.name;
                GetTextComponentOfChild(qualityPanel, "CurrentTimeHolder").text = qualityData.coinPerSecond.ToString();
            }
        }

        GetButtonComponentOfChild(qualityPanel, "BuyButton").interactable = false;

        if (NextQualityMerchData != null)
        {
            MerchData qualityData = NextQualityMerchData();
            if (qualityData != null)
            {
                GetButtonTextComponentOfChild(qualityPanel, "BuyButton").text = "Buy " + qualityData.name;
                GetTextComponentOfChild(qualityPanel, "NextBoostProperties").text = "It'll give " + qualityData.coinPerSecond;

                if (CanBuy != null)
                {
                    Button buyButton = GetButtonComponentOfChild(qualityPanel, "BuyButton");
                    buyButton.interactable = CanBuy(qualityData.upgradeCost);
                    // FIXME: temporary solution
                    buyButton.onClick.RemoveAllListeners();
                    buyButton.onClick.AddListener(() => BuyQualityMerch(qualityData));
                }
            }
            
        }

        // this duplicate is not nice at all, I'll fix this later

        if (CurrentTimeMerchData != null)
        {
            MerchData timeData = CurrentTimeMerchData();
            if (timeData != null)
            {
                GetTextComponentOfChild(timePanel, "CurrentLevelHolder").text = timeData.level.ToString();
                GetTextComponentOfChild(timePanel, "CurrentNameHolder").text = timeData.name;
                GetTextComponentOfChild(timePanel, "CurrentTimeHolder").text = timeData.timeLimit.ToString();
            }
        }

        GetButtonComponentOfChild(timePanel, "BuyButton").interactable = false;

        if (NextTimeMerchData != null)
        {
            MerchData timeData = NextTimeMerchData();
            if (timeData != null)
            {
                GetButtonTextComponentOfChild(timePanel, "BuyButton").text = "Buy " + timeData.name;
                GetTextComponentOfChild(timePanel, "NextBoostProperties").text = "It'll give " + timeData.timeLimit;

                if (CanBuy != null)
                {
                    Button buyButton = GetButtonComponentOfChild(timePanel, "BuyButton");
                    buyButton.interactable = CanBuy(timeData.upgradeCost);
                    // FIXME: temporary solution
                    buyButton.onClick.RemoveAllListeners();
                    buyButton.onClick.AddListener(() => BuyTimeMerch(timeData));
                }
            }

        }
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
