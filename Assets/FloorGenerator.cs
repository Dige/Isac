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

    private const float HorizontalDelta = 16.95f;
    private const float VerticalDelta = 10.5f;

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
            var direction = (RoomCreationDirection)Random.Range(0, 3);
            switch (direction)
            {
                case RoomCreationDirection.North:
                    coordinates = Tuple.Create(coordinates.Item1, coordinates.Item2 + 1);
                    break;
                case RoomCreationDirection.East:
                    coordinates = Tuple.Create(coordinates.Item1 + 1, coordinates.Item2);
                    break;
                case RoomCreationDirection.South:
                    coordinates = Tuple.Create(coordinates.Item1, coordinates.Item2 - 1);
                    break;
                case RoomCreationDirection.West:
                    coordinates = Tuple.Create(coordinates.Item1  - 1, coordinates.Item2);
                    break;
            }
            if (!_floorGrid.ContainsRoom(coordinates.Item1, coordinates.Item2))
            {
                previousRoom = CreateRoom(previousRoom, direction);
                i++;
            }
        }
    }

    private Room CreateRoom(Room previousRoom, RoomCreationDirection direction)
    {
        var room = (Room)Instantiate(RoomPrefabs.First());
        if (previousRoom != null)
        {
            var position = previousRoom.transform.position;
            switch (direction)
            {
                case RoomCreationDirection.North:
                    position.y += VerticalDelta;
                    break;
                case RoomCreationDirection.East:
                    position.x += HorizontalDelta;
                    break;
                case RoomCreationDirection.South:
                    position.y -= VerticalDelta;
                    break;
                case RoomCreationDirection.West:
                    position.x -= HorizontalDelta;
                    break;
            }
            room.transform.position = position;
        }
        return room;
    }

    private enum RoomCreationDirection
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
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
}
