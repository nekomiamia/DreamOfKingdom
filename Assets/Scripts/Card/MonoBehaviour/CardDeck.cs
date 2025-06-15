using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using Utilities;


public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    public Vector3 deckPos; // 手牌起始位置
    private List<CardDataSO> drawDeck = new(); // 抽牌堆
    private List<CardDataSO> discardDeck = new(); // 弃牌堆

    private List<Card> handCardObjectList = new List<Card>(); // 当前手牌（每回合不一样

    [Header("Events")]
    public IntEventSO drawCardEvent; // 抽牌事件
    public IntEventSO discardCardEvent; // 弃牌事件
    
    private void Start()
    {
        // 测试
        Init(); 
    }

    public void Init()
    {
        drawDeck.Clear();
        foreach (var entry in cardManager.currentCardLib.cardLibList)
        {
            for(int i = 0; i < entry.count; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }
        
        // TODO 洗牌/更新UI
        ShuffleDeck();
        
    }

    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }

    /// <summary>
    /// Event Func
    /// </summary>
    public void NewTurnDrawCard()
    {
        DrawCard(4);
    }
    
    
    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(drawDeck.Count == 0)
            {
                foreach (var cardDataSo in discardDeck)
                {
                    drawDeck.Add(cardDataSo);
                }
                ShuffleDeck();
            }

            CardDataSO curCardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            // Update UI
            drawCardEvent.RaiseEvent(drawDeck.Count, this);
            
            var card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(curCardData);
            card.transform.position = deckPos;
            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }


    // 重新设置手牌布局
    public void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card curCard = handCardObjectList[i];
            curCard.isAnimating = true;
            CardTransform curCardTransform = cardLayoutManager.GetCardTransform(i, handCardObjectList.Count);

            curCard.UpdateCardCost();
            
            curCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).onComplete = () =>
            {
                curCard.transform.DOMove(curCardTransform.pos, 0.5f).onComplete = () => curCard.isAnimating = false;
                curCard.transform.DORotateQuaternion(curCardTransform.rotation, 0.5f);
            };
            
            // 设置排序
            curCard.GetComponent<SortingGroup>().sortingOrder = i; // 确保手牌的渲染顺序正确
            curCard.UpdatePositionRotation(curCardTransform.pos, curCardTransform.rotation); 
            
        }
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleDeck()
    {
        discardDeck.Clear();
        
        drawCardEvent.RaiseEvent(drawDeck.Count, this);
        discardCardEvent.RaiseEvent(discardDeck.Count, this);
        
        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = UnityEngine.Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }
    
    /// <summary>
    /// Discard Card, Event Func
    /// </summary>
    /// <param name="card"></param> 
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);
        cardManager.DiscardCardObject(card.gameObject);
        
        discardCardEvent.RaiseEvent(discardDeck.Count, this);
        SetCardLayout(0f);
    }

    /// <summary>
    /// Event Func
    /// </summary>
    public void OnPlayerTurnEnd()
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            discardDeck.Add(handCardObjectList[i].cardData);
            cardManager.DiscardCardObject(handCardObjectList[i].gameObject);
        }
        
        handCardObjectList.Clear();
        discardCardEvent.RaiseEvent(discardDeck.Count, this);
    }

    public void ReleaseAllCards(object obj)
    {
        foreach (var card in handCardObjectList)
        {
            cardManager.DiscardCardObject(card.gameObject);
        }
        handCardObjectList.Clear();
        
        Init();
    }
}
