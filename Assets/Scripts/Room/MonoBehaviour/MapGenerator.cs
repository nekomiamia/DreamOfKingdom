using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;


public class MapGenerator : MonoBehaviour
{
    public MapConfigSO mapConfig; // 地图配置SO
    [Header("房间布局存储")] 
    public MapLayoutSO mapLayout;
    [Header("预制体")]
    public Room roomPrefab; // 房间预制体

    public LineRenderer lineRenderer;
    private float screenWidth;
    private float screenHeight;
    public float columnWidth;

    private Vector3 generatePoint;
    public float border;

    private List<Room> rooms = new List<Room>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    public List<RoomDataSO> roomDataList = new List<RoomDataSO>();
    private Dictionary<RoomType, RoomDataSO> roomDataDict = new Dictionary<RoomType, RoomDataSO>();
    
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;
        
        columnWidth = screenWidth / (mapConfig.RoomBlueprints.Count);

        foreach (var roomDataSo in roomDataList)
        {
            roomDataDict.Add(roomDataSo.roomType, roomDataSo);
        }
    }

    private void OnEnable()
    {
        if (mapLayout.mapRoomDataList.Count > 0)
        {
            LoadMap();
        }else 
        {
            CreateMap();
        }
    }

    private void Start()
    {
        // CreateMap();
    }

    public void CreateMap()
    {
        List<Room> preColumnRooms = new List<Room>();
        
        
        for (int column = 0; column < mapConfig.RoomBlueprints.Count; column++)
        {
            var blueprint = mapConfig.RoomBlueprints[column];
            var amount = Random.Range(blueprint.min, blueprint.max + 1);

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);
            generatePoint = new Vector3(-screenWidth / 2 + border + columnWidth * column, startHeight, 0);

            var newPosition = generatePoint;
    
            List<Room> curColumnRooms = new List<Room>();
            
            var roomGapY = screenHeight / (amount + 1);
            for (int i = 0; i < amount; i++)
            {
                // Boss
                if (column == mapConfig.RoomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 2;
                }else if (column != 0)
                {
                    newPosition.x = generatePoint.x + Random.Range(-border / 2, border / 2);
                }
                newPosition.y = startHeight - i * roomGapY;
                // 生成房间
                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity, transform);
                RoomType type = GetRandomRoomType(mapConfig.RoomBlueprints[column].roomType);

                if (column == 0) room.RoomState = RoomState.Attainable;
                else room.RoomState = RoomState.Locked;
                
                room.SetupRoom(column, i, GetRoomData(type));
                
                rooms.Add(room);
                curColumnRooms.Add(room);
            }

            if (preColumnRooms.Count > 0)
            {
                //创建连接线
                CreateConnectionLine(preColumnRooms, curColumnRooms);
            }

            preColumnRooms = curColumnRooms;
        }
        
        SaveMap();
    }

    private void CreateConnectionLine(List<Room> preColumnRooms, List<Room> curColumnRooms)
    {
        HashSet<Room> connectedCurRooms = new HashSet<Room>();

        foreach (var room in preColumnRooms)
        {
            var target = ConnectToRandomRoom(room, curColumnRooms, false);
            connectedCurRooms.Add(target);
        }

        foreach (var room in curColumnRooms)
        {
            if (!connectedCurRooms.Contains(room))
            {
                ConnectToRandomRoom(room, preColumnRooms, true);
            }
        }
    }

    private Room ConnectToRandomRoom(Room room, List<Room> curColumnRooms, bool check)
    {
        Room targetRoom = null;
        
        targetRoom = curColumnRooms[Random.Range(0, curColumnRooms.Count)];
        if (check) // 如果反向连接
        {
            targetRoom.linkTo.Add(new Vector2Int(room.column, room.line));
        }
        else
        {
            room.linkTo.Add(new Vector2Int(targetRoom.column, targetRoom.line)); 
        }
        
        var line = Instantiate(lineRenderer, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        
        lines.Add(line);
        return targetRoom;
    }

    [ContextMenu("ReGenerate Map")]
    public void ReGenerateMap()
    {
        // 清除旧房间
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        rooms.Clear();
        lines.Clear();
        // 重新生成地图
        CreateMap();
    }


    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');
        
        string randomOption = options[Random.Range(0, options.Length)];

        RoomType roomType = (RoomType)Enum.Parse(typeof(RoomType), randomOption);

        return roomType;
    }


    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new();
        for (int i = 0; i < rooms.Count; i++)
        {
            var room = new MapRoomData()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                column = rooms[i].column,
                line = rooms[i].line,
                roomData = rooms[i].roomData,
                roomState = rooms[i].RoomState,
                linkTo = rooms[i].linkTo
            };
            
            mapLayout.mapRoomDataList.Add(room);
        }
        
        // 连线部分
        mapLayout.linePositionList = new List<LinePosition>();
        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition()
            {
                startPos = new SerializeVector3(lines[i].GetPosition(0)),
                endPos = new SerializeVector3(lines[i].GetPosition(1))
            };
            mapLayout.linePositionList.Add(line);
        }
    }

    private void LoadMap()
    {
        // 读取房间数据生成房间
        foreach (var mapRoomData in mapLayout.mapRoomDataList)
        {
            var room = Instantiate(roomPrefab, new Vector3(mapRoomData.posX, mapRoomData.posY, 0), Quaternion.identity, transform);
            room.RoomState = mapRoomData.roomState;
            room.SetupRoom(mapRoomData.column, mapRoomData.line, mapRoomData.roomData);
            room.linkTo = mapRoomData.linkTo;
            rooms.Add(room);
        }

        foreach (var lineData in mapLayout.linePositionList)
        {
            var line = Instantiate(lineRenderer, transform);
            line.SetPosition(0, lineData.startPos.ToVector3());
            line.SetPosition(1, lineData.endPos.ToVector3());
            lines.Add(line);
        }

    }
}
