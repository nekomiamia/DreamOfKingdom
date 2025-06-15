using System.Collections.Generic;
using UnityEngine;
using Utilities;


[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO", order = 0)]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite cardImage; // 卡牌图片
    public int cost;
    public CardType cardType; // 卡牌类型
    [TextArea]
    public string description; // 卡牌描述
    
    
    // 执行的实际效果
    public List<Effect> effects;
    
    
}
