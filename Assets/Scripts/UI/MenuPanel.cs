using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button startButton, exitButton;

    public ObjectEventSO onStartGameEvent;
    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        startButton = rootElement.Q<Button>("StartButton");
        exitButton = rootElement.Q<Button>("ExitButton");
        
        startButton.clicked += OnStartButtonClicked;
        exitButton.clicked += OnExitButtonClicked;
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void OnStartButtonClicked()
    {
        onStartGameEvent.RaiseEvent(null, this);
    }
}
