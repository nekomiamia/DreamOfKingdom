using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;


public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout; // 地图布局SO
    public List<Enemy> aLiveEnemies = new List<Enemy>(); // 存活的敌人列表

    [Header("广播事件")] public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameLoseEvent;
    /// <summary>
    /// 更新房间的监听函数，重新加载地图
    /// </summary>
    /// <param name="roomVector"></param>
    public void UpdateMapLayoutData(object roomVector)
    {
        var roomVec = (Vector2Int)roomVector;
        var curRoom = mapLayout.mapRoomDataList.Find(r => r.column == roomVec.x&& r.line == roomVec.y);

        curRoom.roomState = RoomState.Visited;
        // 更新同一列的房间状态为锁定
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.column == roomVec.x);
        foreach (var room in sameColumnRooms)
        {
            if (room.line!=roomVec.y)
            {
                room.roomState = RoomState.Locked;
            }
        }

        foreach (Vector2Int link in curRoom.linkTo)
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(r => r.column == link.x && r.line == link.y);
            linkedRoom.roomState = RoomState.Attainable;
        }
        
        aLiveEnemies.Clear(); // 清空存活敌人列表
    }

    public void OnRoomLoadedEvent(object obj)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            aLiveEnemies.Add(enemy);
        }
    }
    
    
    public void OnCharacterDeathEvent(object obj)
    {
        if (obj is Player)
        {
            // 发出失败的通知
            StartCoroutine(EventDelayAction(gameLoseEvent));
        }

        if (obj is Enemy)
        {
            aLiveEnemies.Remove(obj as Enemy);
            
            if (aLiveEnemies.Count == 0)
            {
                // 发出胜利的通知
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }
    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaiseEvent(null, this);
    }
}
