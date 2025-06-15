using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference curScene;
    public AssetReference map;
    public Room curRoom;
    private Vector2Int curRoomVec;
    
    [Header("广播")]
    public ObjectEventSO afterRoomLoadedEvent;

    public ObjectEventSO updateRoomEvent;

    private void Start()
    {
        curRoomVec = Vector2Int.one * -1; // 初始化为-1，表示未加载房间
    }

    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            curRoom = data as Room;
            var curRoomData = curRoom.roomData;
            curRoomVec = new Vector2Int(curRoom.column, curRoom.line);
            
            curScene = curRoomData.sceneToLoad;
        }

        await UnloadSceneTask();
        await LoadSceneTask();
        afterRoomLoadedEvent.RaiseEvent(curRoom, this);
    }


    private async Task LoadSceneTask()
    {
        var s = curScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;

        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }
    
    private async Task UnloadSceneTask()
    {
        var unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        while (!unloadOperation.isDone)
        {
            await Task.Yield();
        }
    }

    public async void LoadMap()
    {
        await UnloadSceneTask();
        if (curRoomVec != Vector2.one * -1)
        {
            updateRoomEvent.RaiseEvent(curRoomVec, this);
        }
        curScene = map;
        await LoadSceneTask();
    }
}
