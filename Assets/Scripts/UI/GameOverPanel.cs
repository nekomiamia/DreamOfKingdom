using System;
using UnityEngine;
using UnityEngine.UIElements;


public class GameOverPanel : MonoBehaviour
{
    private Button backToStartButton;
    public ObjectEventSO loadMenuEvent;
    
    private void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStartButton").clicked +=
            BackToStartButton;
    }

    private void BackToStartButton()
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
