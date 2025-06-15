using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    public VisualElement cardContainer;

    private CardDataSO curCardData;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");

        for (int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            cardContainer.Add(card);
            card.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
            card.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
            Debug.Log($"Card {i + 1} instantiated.");
        }
    }

    private void Start()
    {
        
    }
}
