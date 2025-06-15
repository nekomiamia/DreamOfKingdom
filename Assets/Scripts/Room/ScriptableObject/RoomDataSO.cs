using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities;

[CreateAssetMenu(fileName = "RoomDataSO", menuName = "Map/RoomDataSO")]
public class RoomDataSO : ScriptableObject
{
    public Sprite roomIcon;
    public RoomType roomType;
    public AssetReference sceneToLoad;
}
