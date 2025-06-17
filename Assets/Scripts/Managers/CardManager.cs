
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utilities;
using System.Linq;

public class CardManager: MonoBehaviour
{

    public PoolTool poolTool;
    public List<CardDataSO> cardDataList; // 卡牌数据列表

    [Header("卡牌库")]
    public CardLibSO newGameCardLib; // 新游戏时的卡牌库
    public CardLibSO currentCardLib; // 当前玩家的卡牌库

    private int preIndex = 0;
    private void Awake()
    {
        InitCardDataList();

        foreach (CardLibEntry entry in newGameCardLib.cardLibList)
        {
            Debug.Log($"卡牌库初始化: {entry.cardData.name}, 数量: {entry.count}");
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

    public CardDataSO GetNewCardData()
    {
        var randomIndex = 0;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, cardDataList.Count);
        } while (preIndex == randomIndex);
        
        preIndex = randomIndex;
        return cardDataList[randomIndex];
    }

    public void UnlockCard(CardDataSO cardDataSo)
    {
        var newCard = new CardLibEntry()
        {
            cardData = cardDataSo,
            count = 1
        };

        var index = currentCardLib.cardLibList.FindIndex(entry => entry.cardData == cardDataSo);
        if (index >= 0)
        {
            var existingEntry = currentCardLib.cardLibList[index];
            existingEntry.count++;
            currentCardLib.cardLibList[index] = existingEntry; // 更新列表中的结构体
            Debug.Log($"卡牌 {cardDataSo.name} 已解锁，当前数量: {existingEntry.count}");
        }
        else
        {
            currentCardLib.cardLibList.Add(newCard);
        }
    }
}
