using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

public class PickCardPanel : MonoBehaviour
{
    public CardManager CardManager;
    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    public VisualElement cardContainer;

    private CardDataSO curCardData;
    private Button confirmButton; // 确认按钮
    private List<Button> cardButtons = new List<Button>(); // 存储卡牌按钮列表

    [Header("广播事件")]
    public ObjectEventSO pickCardEvent; // 选择卡牌事件
    
    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmButton");

        confirmButton.clicked += OnConfirmClick;
        
        for (int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            var data = CardManager.GetNewCardData();
            InitCard(card, data);
            var cardButton = card.Q<Button>("Card");
            
            cardContainer.Add(card);
            cardButtons.Add(cardButton);
            cardButton.clicked += () => OnCardClick(cardButton, data);
            card.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
            card.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
            Debug.Log($"Card {i + 1} instantiated.");
        }
    }

    private void OnConfirmClick()
    {
        CardManager.UnlockCard(curCardData);
        pickCardEvent.RaiseEvent(null, this);
    }

    private void OnCardClick(Button cardButton, CardDataSO data)
    {
        curCardData = data;
        Debug.Log($"Card {data.cardName} clicked.");
        for (int i = 0; i < cardButtons.Count; i++)
        {
            if (cardButtons[i] == cardButton)
            {
                cardButtons[i].SetEnabled(false);
            }else 
            {
                cardButtons[i].SetEnabled(true);
            }
        }
    }

    public void InitCard(VisualElement card, CardDataSO cardData)
    {
        
        var cardSpriteElement = card.Q<VisualElement>("CardSprite");
        var cardNameElement = card.Q<Label>("CardNameLabel");
        var cardDescriptionElement = card.Q<Label>("DesLabel");
        var cardCostElement = card.Q<Label>("CostLabel");
        var cardTypeElement = card.Q<Label>("TypeLabel");
        
        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
        cardNameElement.text = cardData.cardName;
        cardDescriptionElement.text = cardData.description;
        cardCostElement.text = cardData.cost.ToString();
        cardTypeElement.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Ability => "能力",
            _ => "未知类型"
        };
        
    }
}
