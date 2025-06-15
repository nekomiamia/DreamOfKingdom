using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Room : MonoBehaviour
{
    public int column; // 纵
    public int line; // 横
    
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState RoomState;

    public List<Vector2Int> linkTo = new List<Vector2Int>();

    public ObjectEventSO loadRoomEvent;
    
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("room data: " + roomData.roomType);
        if(RoomState == RoomState.Attainable)
            loadRoomEvent.RaiseEvent(this, this);
    }
    /// <summary>
    /// 外部根据配置创建房间时调用
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    /// <param name="roomData"></param>
    public void SetupRoom(int column, int line, RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;
        spriteRenderer.sprite = roomData.roomIcon;
        spriteRenderer.color = RoomState switch
        {
            RoomState.Locked => Color.gray,
            RoomState.Visited => Color.green,
            RoomState.Attainable => Color.yellow,
            _ => Color.white
        };
    }
}
