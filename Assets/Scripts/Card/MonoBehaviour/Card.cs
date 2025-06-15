using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Utilities;


public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("组件")] 
    public SpriteRenderer cardSprite;

    public TextMeshPro costText, desText, TypeText, nameText; 
    public CardDataSO cardData;

    [Header("原始数据/Slot")] 
    public Vector3 originPos;
    public Quaternion originRotation;
    public int originLayerOrder;
    public bool isAnimating;
    public bool isAvailiable;
    public Player player;

    [Header("广播事件")] public ObjectEventSO discardCardEvent;
    public IntEventSO costEvent; // 用于扣除费用
    private void Start()
    {
        Init(cardData);
    }


    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardImage;
        costText.text = data.cost.ToString();
        desText.text = data.description;
        nameText.text = data.cardName;
        TypeText.text = data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Ability => "能力",
            _ => "未知类型"
        };
        
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void UpdatePositionRotation(Vector3 pos, Quaternion rotation)
    {
        originPos = pos;
        originRotation = rotation;
        originLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isAnimating) return;
        transform.position = originPos + Vector3.up;
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isAnimating) return;
        ResetCardTransform();
    }

    public void ResetCardTransform()
    {
        transform.SetPositionAndRotation(originPos, originRotation);
        GetComponent<SortingGroup>().sortingOrder = originLayerOrder;
    }


    public void ExecuteCardEffects(CharacterBase from, CharacterBase target)
    {
        costEvent.RaiseEvent(cardData.cost, this);
        discardCardEvent.RaiseEvent(this, this);
        foreach (var effect in cardData.effects)
        {
            effect.Execute(from, target);
        }
    }

    public void UpdateCardCost()
    {
        isAvailiable = cardData.cost <= player.CurMana;
        costText.color = isAvailiable ? Color.green : Color.red;
    }
}
