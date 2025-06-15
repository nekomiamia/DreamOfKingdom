using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CardLibSO", menuName = "Card/CardLibSO", order = 0)]
public class CardLibSO : ScriptableObject
{
    public List<CardLibEntry> cardLibList; // 卡牌条目列表
}

[Serializable]
public struct CardLibEntry
{
    public CardDataSO cardData; // 卡牌数据
    public int count; // 卡牌数量
}