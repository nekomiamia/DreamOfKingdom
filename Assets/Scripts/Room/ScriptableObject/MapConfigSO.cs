using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "MapConfigSO", menuName = "Map/MapConfigSO", order = 0)]
public class MapConfigSO : ScriptableObject
{
    public List<RoomBlueprint> RoomBlueprints; // 房间蓝图列表
}

[Serializable]
public class RoomBlueprint
{
    public int min, max;
    public RoomType roomType;
}