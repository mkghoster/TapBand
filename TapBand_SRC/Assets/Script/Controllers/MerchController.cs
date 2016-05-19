using UnityEngine;
using System.Collections;
using System;

public class MerchController : MonoBehaviour {

    private float currentTime;

    public delegate void MerchTransactionEvent(MerchData merch);
    public event MerchTransactionEvent MerchTransaction;
    public delegate void CoinTransactionEvent(int coins);
    public event CoinTransactionEvent CoinTransaction;
    public delegate bool CanBuyEvent(int price);
    public event CanBuyEvent CanBuy;

    private MerchUI merchUI;
    
    void Awake()
    {
        merchUI = (MerchUI)FindObjectOfType(typeof(MerchUI));
    }

    void Start()
    {
        
    }

    void Update()
    {
        TickSecondsAndCollectMoney();
    }

    private void TickSecondsAndCollectMoney()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 1f)
        {
            currentTime = 0f;
            CollectMoney(1);
        }
    }

    private void CollectMoney(int forSeconds)
    {
        
    }

    void OnEnable()
    {
        merchUI.NextTimeMerchData += NextTimeMerchData;
        merchUI.BuyQualityMerch += BuyQualityMerch;
        merchUI.BuyTimeMerch += BuyTimeMerch;
        merchUI.CanBuy += CanBuyItem;
    }

    void OnDisable()
    {
            merchUI.NextTimeMerchData -= NextTimeMerchData;
        merchUI.BuyQualityMerch -= BuyQualityMerch;
        merchUI.BuyTimeMerch -= BuyTimeMerch;
        merchUI.CanBuy -= CanBuyItem;

        // save last online date
        GameState.instance.Merch.LastOnlineDate = DateTime.Now;
    }

    

    

    private MerchData NextTimeMerchData()
    {
        return null;
        //if (GameState.instance.Merch.CurrentTimeMerch == null)
        //{
        //    return GameData.instance.MerchDataList.Find(x => x.merchType == MerchType.TIME); // finds the first
        //}
        //MerchData nextMerch = ListUtils.NextOf(GameData.instance.MerchDataList, CurrentTimeMerchData());
        //return nextMerch.merchType == MerchType.TIME ? nextMerch : null;
    }

    private void BuyQualityMerch(MerchData data)
    {
        if (MerchTransaction != null)
        {
            MerchTransaction(data);
            GameState.instance.Merch.QualityMerchId = data.id;
        }
    }

    private void BuyTimeMerch(MerchData data)
    {
        if (MerchTransaction != null)
        {
            MerchTransaction(data);
            GameState.instance.Merch.TimeMerchId = data.id;
        }
    }


    private bool CanBuyItem(int price)
    {
        if (CanBuy != null)
        {
            return CanBuy(price);
        }
        return false;
    }
}
