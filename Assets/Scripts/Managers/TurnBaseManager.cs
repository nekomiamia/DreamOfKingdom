using System;
using UnityEngine;
using Utilities;


public class TurnBaseManager : MonoBehaviour
{
    private bool isPlayerTurn = false;
    private bool isEnemyTurn = false;
    public bool battleEnd = true;
    
    private float timeCounter;
    public float enemyTurnDuration;
    public float playerTurnDuration;

    public Player player;
    [Header("Events")] public ObjectEventSO playerTurnBegin;
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;
    
    private void Update()
    {
        if(battleEnd)
            return;

        if (isEnemyTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enemyTurnDuration)
            {
                timeCounter = 0f;
                // 结束敌人回合
                EnemyTurnEnd();
                // Player Turn
                isPlayerTurn = true;
            }
        }

        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0f;
                // Player Turn
                PlayerTurnBegin(); 
                isPlayerTurn = false;
            }
        }
    }

    [ContextMenu("GameStart")]
    public void GameStart()
    {
        battleEnd = false;
        isPlayerTurn = true;
        isEnemyTurn = false;
        timeCounter = 0f;
    }
    
    
    public void PlayerTurnBegin()
    {
        player.NewTurn();
        playerTurnBegin.RaiseEvent(null, this);
    }

    /// <summary>
    /// Enemy Event Func
    /// </summary>
    public void EnemyTurnBegin()
    {
        isEnemyTurn = true;
        enemyTurnBegin.RaiseEvent(null, this);
    }
    
    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        enemyTurnEnd.RaiseEvent(null, this);
    }

    /// <summary>
    /// 房间加载之后
    /// </summary>
    /// <param name="obj"></param>
    public void OnRoomLoadedEvent(object obj)
    {
        Room room = obj as Room;
        switch (room.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                player.gameObject.SetActive(true);
                GameStart();
                break;
            case RoomType.Shop:
                player.gameObject.SetActive(false);
                break;
            case RoomType.Treasure:
                player.gameObject.SetActive(false);
                break;
            case RoomType.RestRoom:
                player.gameObject.SetActive(true);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnChDeathEvent()
    {
        battleEnd = true;
    }

    public void OnLoadMapEvent()
    {
        battleEnd = true;
        player.gameObject.SetActive(false);
    }
}
