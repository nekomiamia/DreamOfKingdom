
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utilities;

public class CardManager: MonoBehaviour
{

    public PoolTool poolTool;
    public List<CardDataSO> cardDataList; // 卡牌数据列表

    [Header("卡牌库")]
    public CardLibSO newGameCardLib; // 新游戏时的卡牌库
    public CardLibSO currentCardLib; // 当前玩家的卡牌库
    private void Awake()
    {
        InitCardDataList();

        foreach (CardLibEntry entry in newGameCardLib.cardLibList)
        {
            currentCardLib.cardLibList.Add(entry);
        }
    }

    private void OnDisable()
    {
        currentCardLib.cardLibList.Clear();
    }

    private void InitCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
    }

    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(obj.Result);
        }else
        {
            Debug.LogError("卡牌数据加载失败: " + obj.OperationException);
        }
    }

    public GameObject GetCardObject()
    {
        var cardObject = poolTool.GetObject();
        cardObject.transform.localScale = Vector3.zero;
        
        return cardObject;
    }
    
    public void DiscardCardObject(GameObject cardObject)
    {
        if (cardObject != null)
        {
            poolTool.ReleaseObject(cardObject);
        }
        else
        {
            Debug.LogWarning("尝试释放一个不在池中的对象或对象为null");
        }
    }
}
