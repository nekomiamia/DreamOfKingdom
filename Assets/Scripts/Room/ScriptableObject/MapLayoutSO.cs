﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;


[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData> mapRoomDataList = new List<MapRoomData>();
    public List<LinePosition> linePositionList = new List<LinePosition>();
}

[Serializable]
public class MapRoomData
{
    public float posX, posY;
    public int column, line;
    public RoomDataSO roomData;
    public RoomState roomState;
    public List<Vector2Int> linkTo = new List<Vector2Int>();
}

[Serializable]
public class LinePosition
{
    public SerializeVector3 startPos, endPos;
}