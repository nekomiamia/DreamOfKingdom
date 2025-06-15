using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button pickCardButton, backToMapButton;

    [Header("事件广播")]
    public ObjectEventSO loadMapEvent; // 加载地图事件
    public ObjectEventSO pickCardEvent; // 选新卡事件

    private void Awake()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        pickCardButton = rootElement.Q<Button>("PickCardButton");
        backToMapButton = rootElement.Q<Button>("BackToMapButton");

        pickCardButton.clicked += OnPickCardButtonClicked;
        backToMapButton.clicked += OnBackToMapButtonClicked;
    }

    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    private void OnPickCardButtonClicked()
    {
        pickCardEvent.RaiseEvent(null, this);
    }
    
    public void OnFinishPickCardEvent()
    {
        pickCardButton.style.display = DisplayStyle.None;
    }
}
