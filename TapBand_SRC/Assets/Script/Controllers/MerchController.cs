using UnityEngine;
using System.Collections;

public class MerchController : MonoBehaviour {

    private MerchUI merchUI;

	void Awake () {
	    merchUI = (MerchUI)FindObjectOfType(typeof(MerchUI));
    }

    void OnEnable()
    {
        merchUI.CurrentQualityMerchData += CurrentQualityMerchData;
        merchUI.NextQualityMerchData += NextQualityMerchData;
        merchUI.CurrentTimeMerchData += CurrentTimeMerchData;
        merchUI.NextTimeMerchData += NextTimeMerchData;
        merchUI.BuyQualityMerch += BuyQualityMerch;
        merchUI.BuyTimeMerch += BuyTimeMerch;
        merchUI.CanBuy += CanBuy;
    }

    void OnDisable()
    {
        merchUI.CurrentQualityMerchData -= CurrentQualityMerchData;
        merchUI.NextQualityMerchData -= NextQualityMerchData;
        merchUI.CurrentTimeMerchData -= CurrentTimeMerchData;
        merchUI.NextTimeMerchData -= NextTimeMerchData;
        merchUI.BuyQualityMerch -= BuyQualityMerch;
        merchUI.BuyTimeMerch -= BuyTimeMerch;
        merchUI.CanBuy -= CanBuy;
    }

    private MerchData CurrentQualityMerchData()
    {
        return GameState.instance.Merch.CurrentQualityMerch;
    }

    private MerchData NextQualityMerchData()
    {
        if (GameState.instance.Merch.CurrentQualityMerch == null)
        {
            return GameData.instance.MerchDataList.Find(x => x.merchType == MerchType.QUALITY); // finds the first
        }
        bool currentFound = false;
        foreach (MerchData data in GameData.instance.MerchDataList)
        {
            if (currentFound && data.merchType == MerchType.QUALITY)
            {
                return data;
            }

            if (data.id == GameState.instance.Merch.QualityMerchId)
            {
                currentFound = true;
            }
        }

        // figure this out
        return CurrentQualityMerchData();
    }

    private MerchData CurrentTimeMerchData()
    {
        return GameState.instance.Merch.CurrentTimeMerch;
    }

    private MerchData NextTimeMerchData()
    {
        if (GameState.instance.Merch.CurrentTimeMerch == null)
        {
            return GameData.instance.MerchDataList.Find(x => x.merchType == MerchType.TIME); // finds the first
        }
        bool currentFound = false;
        foreach (MerchData data in GameData.instance.MerchDataList)
        {
            if (currentFound && data.merchType == MerchType.TIME)
            {
                return data;
            }

            if (data.id == GameState.instance.Merch.TimeMerchId)
            {
                currentFound = true;
            }
        }

        // figure this out
        return CurrentTimeMerchData();
    }

    private void BuyQualityMerch(MerchData data)
    {
        GameState.instance.Merch.QualityMerchId = data.id;
        GameState.instance.Currency.NumberOfCoins -= data.upgradeCost;
    }

    private void BuyTimeMerch(MerchData data)
    {
        GameState.instance.Merch.TimeMerchId = data.id;
        GameState.instance.Currency.NumberOfCoins -= data.upgradeCost;
    }


    private bool CanBuy(int price)
    {
        return price <= GameState.instance.Currency.NumberOfCoins;
    }
}
