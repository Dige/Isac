using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloorGenerator : MonoBehaviour {

    [SerializeField]
    private int _numberOfRooms;
    public int NumberOfRooms
    {
        get { return _numberOfRooms; }
        set { _numberOfRooms = value; }
    }

    [SerializeField]
    private int _numberOfTreasureRooms;
    public int NumberOfTreasureRooms
    {
        get { return _numberOfTreasureRooms; }
        set { _numberOfTreasureRooms = value; }
    }

    [SerializeField]
    private List<Room> _roomPrefabs = new List<Room>();
    public List<Room> RoomPrefabs
    {
        get { return _roomPrefabs; }
        set { _roomPrefabs = value; }
    }

    [SerializeField]
    private Room _firstRoom;
    public Room FirstRoom
    {
        get { return _firstRoom; }
        set { _firstRoom = value; }
    }

    private const float HorizontalDelta = 16f;
    private const float VerticalDelta = 10f;

    private readonly FloorGrid _floorGrid = new FloorGrid(6, 6);

	public void Start()
	{
	    Random.seed = DateTime.Now.Second;
	    GenerateFloorLayout();
	}

    private void GenerateFloorLayout()
    {
        int i = 0;
        Room previousRoom = null;
        var coordinates = Tuple.Create(Random.Range(0, 5), Random.Range(0, 5));
        if (FirstRoom != null)
        {
            previousRoom = (Room) Instantiate(FirstRoom);
            _floorGrid.AddRoom(coordinates.Item1, coordinates.Item2, previousRoom);
            i++;
        }

        while (i < _numberOfRooms)
        {
            var direction = (RoomDirection)Random.Range(0, 3);
            switch (direction)
            {
                case RoomDirection.North:
                    coordinates = Tuple.Create(coordinates.Item1, coordinates.Item2 + 1);
                    break;
                case RoomDirection.East:
                    coordinates = Tuple.Create(coordinates.Item1 + 1, coordinates.Item2);
                    break;
                case RoomDirection.South:
                    coordinates = Tuple.Create(coordinates.Item1, coordinates.Item2 - 1);
                    break;
                case RoomDirection.West:
                    coordinates = Tuple.Create(coordinates.Item1  - 1, coordinates.Item2);
                    break;
            }
            if (!_floorGrid.ContainsRoom(coordinates.Item1, coordinates.Item2))
            {
                var newRoom = CreateRoom(previousRoom, direction);
                if (previousRoom != null)
                {
                    previousRoom.SetAdjacentRoom(newRoom, direction);
                }
                newRoom.SetAdjacentRoom(previousRoom, (RoomDirection)(((int)direction + 2) % 4));
                previousRoom = newRoom;
                i++;
            }
        }
    }

    private Room CreateRoom(Room previousRoom, RoomDirection direction)
    {
        var room = (Room)Instantiate(RoomPrefabs.First());
        if (previousRoom != null)
        {
            var position = previousRoom.transform.position;
            switch (direction)
            {
                case RoomDirection.North:
                    position.y += VerticalDelta;
                    break;
                case RoomDirection.East:
                    position.x += HorizontalDelta;
                    break;
                case RoomDirection.South:
                    position.y -= VerticalDelta;
                    break;
                case RoomDirection.West:
                    position.x -= HorizontalDelta;
                    break;
            }
            room.transform.position = position;
        }
        return room;
    }
}

public class FloorGrid
{
    private readonly Room[,] _rooms;

    public FloorGrid(int height, int width)
    {
        _rooms = new Room[height, width];
    }

    public bool ContainsRoom(int x, int y)
    {
        return _rooms[x, y] != null;
    }

    public void AddRoom(int x, int y, Room room)
    {
        if (ContainsRoom(x, y))
        {
            throw new Exception("The cell already contains a room");
        }
        _rooms[x, y] = room;
    }
}

public enum RoomDirection
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}
