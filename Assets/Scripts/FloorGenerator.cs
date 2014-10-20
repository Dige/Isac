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

    [SerializeField]
    private Room _bossRoom;
    public Room BossRoom
    {
        get { return _bossRoom; }
        set { _bossRoom = value; }
    }
    

    [SerializeField]
    private Player _playerPrefab;
    public Player PlayerPrefab
    {
        get { return _playerPrefab; }
        set { _playerPrefab = value; }
    }

    [SerializeField]
    private float _branchingProbability = 0.6f;
    public float BranchingProbability
    {
        get { return _branchingProbability; }
        set { _branchingProbability = value; }
    }

    [SerializeField]
    private Enemy _enemyPrefab;
    public Enemy EnemyPrefab
    {
        get { return _enemyPrefab; }
        set { _enemyPrefab = value; }
    }
    

    private const float HorizontalDelta = 16f;
    private const float VerticalDelta = 10f;

    private readonly FloorGrid _floorGrid = new FloorGrid(6, 6);
    private readonly Stack<Room> _mainPath = new Stack<Room>();

	public void Awake()
	{
	    Random.seed = DateTime.Now.Second;
	    try
	    {
            GenerateFloorLayout();
	    }
	    catch (Exception ex)
	    {
            Debug.LogError("Failed to create level");
	        Debug.LogException(ex);
	    }
	    
	    AddPlayer();
	}

    private void AddPlayer()
    {
        Instantiate(_playerPrefab);
        _floorGrid.FirstRoom.OnPlayerEntersRoom(_playerPrefab);
    }

    private void GenerateFloorLayout()
    {
        var coordinates = new RoomCoordinates(Random.Range(1, 4), Random.Range(1, 4));
        var firstRoom = CreateFirstRoom(coordinates);

        //Create branch for the first room
        int numberOfRoomsCreated = CreateBranch(firstRoom, coordinates, 0);
        if (Random.Range(0.0f, 1.0f) <= BranchingProbability)
        {
            Debug.Log("Branching!");
            numberOfRoomsCreated = CreateBranch(firstRoom, coordinates, numberOfRoomsCreated);
        }

        var previousRoom = firstRoom;
        _mainPath.Push(firstRoom);
        while (numberOfRoomsCreated < _numberOfRooms-1)
        {
            RoomDirection direction;
            try
            {
                direction = DetermineNextRoomLocation(coordinates);
            }
            catch (Exception ex)
            {
                _mainPath.Pop();
                while (_mainPath.Any() && !_floorGrid.GetValidDirectionsFromRoom(_floorGrid.GetCoordinatesForRoom(_mainPath.Peek())).Any())
                {
                    _mainPath.Pop();
                }
                if (_mainPath.Count == 0)
                {
                    throw new Exception("Everything is ruined", ex);
                }
                previousRoom = _mainPath.Peek();
                direction = DetermineNextRoomLocation(_floorGrid.GetCoordinatesForRoom(previousRoom));
                coordinates = _floorGrid.GetCoordinatesForRoom(previousRoom);
                
            }
            
            coordinates = DetermineNewCoordinates(direction, coordinates);

            if (!_floorGrid.IsDeadEnd(coordinates.X, coordinates.Y))
            {
                previousRoom = AddNewRoom(previousRoom, direction, coordinates);
                _mainPath.Push(previousRoom);
                numberOfRoomsCreated++;
            }
            if (Random.Range(0.0f, 1.0f) <= BranchingProbability)
            {
                Debug.Log("Branching!");
                numberOfRoomsCreated = CreateBranch(previousRoom, coordinates, numberOfRoomsCreated);
            }
            //if (Random.Range(0.0f, 1.0f) >= BranchingProbability*BranchingProbability)
            //{
            //    Debug.Log("Branching again!");
            //    numberOfRoomsCreated = CreateBranch(previousRoom, coordinates, numberOfRoomsCreated);
            //}
        }

        CreateBossRoom(previousRoom, coordinates);
    }

    private int CreateBranch(Room previousRoom, RoomCoordinates coordinates, int numberOfRoomsCreated)
    {
        var branchLength = Random.Range(1, 4);
        var previousBranchRoom = previousRoom;
        var branchCoordinates = coordinates;
        while (branchLength > 0)
        {
            try
            {
                RoomDirection direction = DetermineNextRoomLocation(branchCoordinates);
                branchCoordinates = DetermineNewCoordinates(direction, branchCoordinates);
                if (_floorGrid.CanRoomBeAdded(branchCoordinates.X, branchCoordinates.Y))
                {
                    previousBranchRoom = AddNewRoom(previousBranchRoom, direction, branchCoordinates);
                    numberOfRoomsCreated++;
                }
            }
            catch (Exception)
            {
                Debug.Log("Branching failed :(");
                break;
            }
            branchLength--;
        }
        return numberOfRoomsCreated;
    }

    private Room CreateFirstRoom(RoomCoordinates coordinates)
    {
        Room previousRoom;
        if (FirstRoom != null)
        {
            previousRoom = (Room) Instantiate(FirstRoom);
            _floorGrid.AddRoom(coordinates.X, coordinates.Y, previousRoom);
        }
        else
        {
            previousRoom = (Room) Instantiate(RoomPrefabs.First());
            _floorGrid.AddRoom(coordinates.X, coordinates.Y, previousRoom);
        }
        return previousRoom;
    }

    private RoomDirection DetermineNextRoomLocation(RoomCoordinates coordinates)
    {
        var validDirections = _floorGrid.GetValidDirectionsFromRoom(coordinates.X, coordinates.Y).ToList();
        if (!validDirections.Any())
        {
            throw new Exception("Created dead-end :(");
        }

        return validDirections.ElementAt(Random.Range(0, validDirections.Count()));
    }

    private RoomCoordinates DetermineNewCoordinates(RoomDirection direction, RoomCoordinates previousCoordinates)
    {
        switch (direction)
        {
            case RoomDirection.North:
                return new RoomCoordinates(previousCoordinates.X, previousCoordinates.Y + 1);
            case RoomDirection.East:
                return new RoomCoordinates(previousCoordinates.X + 1, previousCoordinates.Y);
            case RoomDirection.South:
                return new RoomCoordinates(previousCoordinates.X, previousCoordinates.Y - 1);
            case RoomDirection.West:
                return new RoomCoordinates(previousCoordinates.X - 1, previousCoordinates.Y);
            default:
                return previousCoordinates;
        }
    }

    private Room AddNewRoom(Room previousRoom, RoomDirection direction, RoomCoordinates coordinates, bool isBossRoom = false)
    {
        var newRoom = CreateRoom(previousRoom, direction, isBossRoom);
        if (previousRoom != null)
        {
            previousRoom.SetAdjacentRoom(newRoom, direction);
            newRoom.SetAdjacentRoom(previousRoom, GetOppositeRoomDirection(direction));
        }
        _floorGrid.AddRoom(coordinates.X, coordinates.Y, newRoom);

        if (!isBossRoom)
        {
            var enemyCount = Random.Range(0, 4);
            for (int i = 0; i <= enemyCount; i++)
            {
                newRoom.InstantiateEnemy(EnemyPrefab, new Vector2(Random.Range(-3, 3), Random.Range(-3, 3)));
            }
        }

        previousRoom = newRoom;
        return previousRoom;
    }

    private void CreateBossRoom(Room previousRoom, RoomCoordinates coordinates)
    {
        Debug.Log("Room before boss: "+ previousRoom.name);
        var validDirections = _floorGrid.GetValidDirectionsFromRoom(coordinates.X, coordinates.Y).ToList();
        if (!validDirections.Any())
        {
            throw new Exception("Failed to create boss room.");
        }

        var direction = validDirections.ElementAt(Random.Range(0, validDirections.Count()));
        AddNewRoom(previousRoom, direction, DetermineNewCoordinates(direction, coordinates), true);
    }

    private static RoomDirection GetOppositeRoomDirection(RoomDirection direction)
    {
        return (RoomDirection)(((int)direction + 2) % 4);
    }

    private Room CreateRoom(Room previousRoom, RoomDirection direction, bool isBossRoom = false)
    {
        var room = (Room)Instantiate(isBossRoom ? BossRoom : RoomPrefabs.First());
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

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Room FirstRoom { get; private set; }

    public FloorGrid(int height, int width)
    {
        Height = height;
        Width = width;
        _rooms = new Room[height, width];
    }

    public bool ContainsRoom(int x, int y)
    {
        return _rooms[x, y] != null;
    }

    public bool ContainsRoom(RoomCoordinates coordinates)
    {
        return ContainsRoom(coordinates.X, coordinates.Y);
    }

    public bool CanRoomBeAdded(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height && !ContainsRoom(x, y);
    }

    public bool CanRoomBeAdded(RoomCoordinates coordinates)
    {
        return CanRoomBeAdded(coordinates.X, coordinates.Y);
    }

    public bool IsDeadEnd(int x, int y)
    {
        return !CanRoomBeAdded(x + 1, y) && !CanRoomBeAdded(x - 1, y) && !CanRoomBeAdded(x, y + 1) &&
               !CanRoomBeAdded(x, y - 1);
    }

    public bool IsDeadEnd(RoomCoordinates coordinates)
    {
        return IsDeadEnd(coordinates.X, coordinates.Y);
    }

    public void AddRoom(int x, int y, Room room)
    {
        if (ContainsRoom(x, y))
        {
            throw new Exception("The cell already contains a room");
        }
        if (FirstRoom == null)
        {
            FirstRoom = room;
        }
        _rooms[x, y] = room;
        room.name = string.Format((room.IsBossRoom ? "Boss" : "") +"Room ({0},{1})", x, y);
    }

    public void AddRoom(RoomCoordinates coordinates, Room room)
    {
        AddRoom(coordinates.X, coordinates.Y, room);
    }

    public IEnumerable<RoomDirection> GetValidDirectionsFromRoom(int x, int y)
    {
        var validDirections = new List<RoomDirection>();
        if (CanRoomBeAdded(x + 1, y))
        {
            validDirections.Add(RoomDirection.East);
        }
        if (CanRoomBeAdded(x - 1, y))
        {
            validDirections.Add(RoomDirection.West);
        }
        if (CanRoomBeAdded(x, y + 1))
        {
            validDirections.Add(RoomDirection.North);
        }
        if (CanRoomBeAdded(x, y - 1))
        {
            validDirections.Add(RoomDirection.South);
        }
        return validDirections;
    }

    public IEnumerable<RoomDirection> GetValidDirectionsFromRoom(RoomCoordinates coordinates)
    {
        return GetValidDirectionsFromRoom(coordinates.X, coordinates.Y);
    }

    public IEnumerable<RoomDirection> GetValidDirectionsFromRoom(Room room)
    {
        return GetValidDirectionsFromRoom(GetCoordinatesForRoom(room));
    }

    public RoomCoordinates GetCoordinatesForRoom(Room room)
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (_rooms[i, j] != room)
                {
                    return new RoomCoordinates(i, j);
                }
            }
        }
        throw new ArgumentException("Room cannot be found from the grid.");
    }

    //TODO: Do not instantiate rooms until the room is ready
    public void InstantiateRooms()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (_rooms[i, j] != null)
                {
                    
                }
            }
        }
    }
}

public struct RoomCoordinates
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public RoomCoordinates(int x, int y) : this()
    {
        X = x;
        Y = y;
    }
}

public enum RoomDirection
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}
