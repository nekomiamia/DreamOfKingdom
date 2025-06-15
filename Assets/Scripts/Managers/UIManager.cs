using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class UIManager : MonoBehaviour
{
    [Header("面板")] public GameObject gameplayPanel;
    public GameObject gameWinPanel;
    public GameObject gameLosePanel;
    public GameObject pickCardPanel;
    
    public void OnLoadRoomEvent(object data)
    {
        Room curRoom = (Room)data;
        switch (curRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gameplayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Load map / Load menu
    /// 
    /// </summary>
    public void HideAllPanel()
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameLosePanel.SetActive(false);
    }
    
    
    public void OnGameWinEvent()
    {
        HideAllPanel();
        gameWinPanel.SetActive(true);
    }
    
    public void OnGameLoseEvent()
    {
        HideAllPanel();
        gameLosePanel.SetActive(true);
    }
    
    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }
}
