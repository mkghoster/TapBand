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
        if (GameState.instance.Merch.CurrentTimeMerch != null)
        {
            int diffInSeconds = (int)(DateTime.Now - GameState.instance.Merch.LastOnlineDate).TotalSeconds;
            int timeLimit = GameState.instance.Merch.CurrentTimeMerch.timeLimit;
            if (diffInSeconds > timeLimit)
            {
                CollectMoney(timeLimit);
            }
            else
            {
                CollectMoney(diffInSeconds);
            }
        }
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
        if (CoinTransaction != null)
        {
            if (GameState.instance.Merch.CurrentQualityMerch != null)
            {
                CoinTransaction(GameState.instance.Merch.CurrentQualityMerch.coinPerSecond * forSeconds);
            }
        }
    }

    void OnEnable()
    {
        merchUI.CurrentQualityMerchData += CurrentQualityMerchData;
        merchUI.NextQualityMerchData += NextQualityMerchData;
        merchUI.CurrentTimeMerchData += CurrentTimeMerchData;
        merchUI.NextTimeMerchData += NextTimeMerchData;
        merchUI.BuyQualityMerch += BuyQualityMerch;
        merchUI.BuyTimeMerch += BuyTimeMerch;
        merchUI.CanBuy += CanBuyItem;
    }

    void OnDisable()
    {
        merchUI.CurrentQualityMerchData -= CurrentQualityMerchData;
        merchUI.NextQualityMerchData -= NextQualityMerchData;
        merchUI.CurrentTimeMerchData -= CurrentTimeMerchData;
        merchUI.NextTimeMerchData -= NextTimeMerchData;
        merchUI.BuyQualityMerch -= BuyQualityMerch;
        merchUI.BuyTimeMerch -= BuyTimeMerch;
        merchUI.CanBuy -= CanBuyItem;

        // save last online date
        GameState.instance.Merch.LastOnlineDate = DateTime.Now;
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

        MerchData nextMerch = ListUtils.NextOf(GameData.instance.MerchDataList, CurrentQualityMerchData());
        return nextMerch.merchType == MerchType.QUALITY ? nextMerch : null;
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
        MerchData nextMerch = ListUtils.NextOf(GameData.instance.MerchDataList, CurrentTimeMerchData());
        return nextMerch.merchType == MerchType.TIME ? nextMerch : null;
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
