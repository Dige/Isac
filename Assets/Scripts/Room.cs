using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts
{
    public class Room : MonoBehaviour
    {
        private readonly List<Door> _doors = new List<Door>();

        public Door NorthDoor { get; private set; }
        public Door SouthDoor { get; private set; }
        public Door EastDoor { get; private set; }
        public Door WestDoor { get; private set; }

        public int X { get; set; }
        public int Y { get; set; }

        [SerializeField]
        private RoomType _roomType = RoomType.NormalRoom;
        public RoomType RoomType
        {
            get { return _roomType; }
            set { _roomType = value; }
        }

        [SerializeField]
        private Door _doorPrefab;
        public Door DoorPrefab
        {
            get { return _doorPrefab; }
            set { _doorPrefab = value; }
        }

        [SerializeField]
        private Door _bossDoorPrefab;
        public Door BossDoorPrefab
        {
            get { return _bossDoorPrefab; }
            set { _bossDoorPrefab = value; }
        }

        [SerializeField]
        private Door _treasureRoomDoorPrefab;
        public Door TreasureRoomDoorPrefab
        {
            get { return _treasureRoomDoorPrefab; }
            set { _treasureRoomDoorPrefab = value; }
        }

        public bool IsBossRoom
        {
            get { return RoomType == RoomType.BossRoom; }
        }

        [SerializeField]
        private BoxCollider2D _northDoorWall;
        public BoxCollider2D NorthDoorWall
        {
            get { return _northDoorWall; }
            set { _northDoorWall = value; }
        }

        [SerializeField]
        private BoxCollider2D _southDoorWall;
        public BoxCollider2D SouthDoorWall
        {
            get { return _southDoorWall; }
            set { _southDoorWall = value; }
        }

        [SerializeField]
        private BoxCollider2D _eastDoorWall;
        public BoxCollider2D EastDoorWall
        {
            get { return _eastDoorWall; }
            set { _eastDoorWall = value; }
        }

        [SerializeField]
        private BoxCollider2D _westDoorWall;
        public BoxCollider2D WestDoorWall
        {
            get { return _westDoorWall; }
            set { _westDoorWall = value; }
        }

        private List<Enemy> _enemies = new List<Enemy>(); 
        public List<Enemy> Enemies
        {
            get { return _enemies; }
			set { _enemies = value; }
        }

        public bool IsVisibleOnMap { get; set; }
        public bool PlayerHasVisited { get; private set; }

        private GameObject _bossBar;
        public GameObject BossBar
        {
            get { return _bossBar; }
        }

        [SerializeField]
        private GameObject bossBarPrefab;
        public GameObject BossBarPrefab
        {
            set { _bossBar = value; }
        }

        [SerializeField]
        private bool _playerIsInRoom;
        public bool PlayerIsInRoom
        {
            get { return _playerIsInRoom; }
            set
            {
                _playerIsInRoom = value;
                if (value)
                {
                    PlayerHasVisited = true;
                    IsVisibleOnMap = true;
                    if (NorthDoor != null)
                    {
                        NorthDoor.ConnectingRoom.IsVisibleOnMap = true;
                    }
                    if (SouthDoor != null)
                    {
                        SouthDoor.ConnectingRoom.IsVisibleOnMap = true;
                    }
                    if (WestDoor != null)
                    {
                        WestDoor.ConnectingRoom.IsVisibleOnMap = true;
                    }
                    if (EastDoor != null)
                    {
                        EastDoor.ConnectingRoom.IsVisibleOnMap = true;
                    }

                    StartCoroutine(WakeUpEnemies());
                }
               
            }
        }

        IEnumerator WakeUpEnemies()
        {
            yield return new WaitForSeconds(1f);
            _enemies.ForEach(e => e.Enable());
            if (_roomType == RoomType.BossRoom)
            {
                _bossBar = (GameObject)Instantiate(bossBarPrefab);
                _bossBar.transform.position = _bossBar.transform.position + transform.position;
            }
            yield return null;
        }

        public bool ContainsEnemies { get { return _enemies.Count > 0; } }

        public void SetAdjacentRoom(Room room, RoomDirection direction)
        {
            var position = new Vector3();
            var rotation = new Quaternion();
            Door doorPrefab = DoorPrefab;
            if (RoomType == RoomType.NormalRoom || RoomType == RoomType.StartRoom)
            {
                switch (room.RoomType)
                {
                    case RoomType.StartRoom:
                        doorPrefab = DoorPrefab;
                        break;
                    case RoomType.NormalRoom:
                        doorPrefab = DoorPrefab;
                        break;
                    case RoomType.BossRoom:
                        doorPrefab = BossDoorPrefab;
                        break;
                    case RoomType.TreasureRoom:
                        doorPrefab = TreasureRoomDoorPrefab;
                        break;
                    case RoomType.SecretRoom:
                        doorPrefab = DoorPrefab;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (RoomType == RoomType.BossRoom)
            {
                doorPrefab = BossDoorPrefab;
            }
            else if (RoomType == RoomType.TreasureRoom)
            {
                doorPrefab = TreasureRoomDoorPrefab;
            }
            else if (RoomType == RoomType.SecretRoom)
            {
                doorPrefab = DoorPrefab;
            }

            var door = (Door)Instantiate(doorPrefab);
            switch (direction)
            {
                case RoomDirection.North:
                    position += new Vector3(0, 4);
                    NorthDoor = door;
                    door.Wall = NorthDoorWall;
                    break;
                case RoomDirection.East:
                    position += new Vector3(7, 0);
                    rotation = Quaternion.Euler(0, 0, 270);
                    EastDoor = door;
                    door.Wall = EastDoorWall;
                    break;
                case RoomDirection.South:
                    position += new Vector3(0, -4);
                    rotation = Quaternion.Euler(0, 0, 180);
                    SouthDoor = door;
                    door.Wall = SouthDoorWall;
                    break;
                case RoomDirection.West:
                    position += new Vector3(-7, 0);
                    rotation = Quaternion.Euler(0, 0, 90);
                    WestDoor = door;
                    door.Wall = WestDoorWall;
                    break;
            }

            door.ConnectingRoom = room;
            door.Direction = direction;
            door.transform.parent = transform;
            door.transform.localPosition = position;
            door.transform.rotation = rotation;
            door.OwnerRoom = this;

            _doors.Add(door);
        }

        public void InstantiateEnemy(Enemy enemyPrefab, Vector2 positionInRoom)
        {   
            var enemy = (Enemy) Instantiate(enemyPrefab);
            enemy.transform.parent = transform;
            enemy.transform.localPosition = Vector3.zero + new Vector3(positionInRoom.x, positionInRoom.y, 0);
            enemy.OwnerRoom = this;
            _enemies.Add(enemy);
            enemy.Disable();
        }

		public void AddEnemy(Enemy enemy, Vector2 positionInRoom)
		{   
			enemy.transform.parent = transform;
			enemy.transform.localPosition = Vector3.zero + new Vector3(positionInRoom.x, positionInRoom.y, 0);
			enemy.OwnerRoom = this;
			_enemies.Add(enemy);
			enemy.Disable();
		}

		public void OnEnemyDied(Enemy enemy)
		{
			_enemies.Remove(enemy);
            if (!ContainsEnemies)
            {
                _doors.ForEach(d => d.IsOpen = true);
                Destroy(_bossBar);
                SpawnItem();
            }
        }

        private void SpawnItem()
        {
            if (RoomType != RoomType.NormalRoom)
            {
                return;
            }
            var spawner = GetComponentInChildren<ItemSpawner>();
            if (spawner != null)
            {
                spawner.Spawn();
            }
            else
            {
                Debug.LogError("Item Spawner missing");
            }

        }

        public void OnPlayerEntersRoom(Player player)
        {
            PlayerIsInRoom = true;
            if (ContainsEnemies)
            {
                _doors.ForEach(d => d.IsOpen = false);
                var doorOpenClip = _doors.First().DoorOpenClip;
                if (doorOpenClip != null)
                {
                    doorOpenClip.Play();
                }
            }
            else
            {
                _doors.ForEach(d => d.IsOpen = true);
                var doorCloseClip = _doors.First().DoorCloseClip;
                if (doorCloseClip != null)
                {
                    doorCloseClip.Play();
                }
            }
            player.CurrentRoom = this;
            Debug.Log("Player entered room: " + name);
        }
    }
}

public enum RoomType
{
    StartRoom,
    NormalRoom,
    BossRoom,
    TreasureRoom,
    SecretRoom
}