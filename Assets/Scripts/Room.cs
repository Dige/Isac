using System.Collections.Generic;
using System.Linq;
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
        private bool _isBossRoom;
        public bool IsBossRoom
        {
            get { return _isBossRoom; }
            set { _isBossRoom = value; }
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

        [SerializeField]
        private readonly List<Enemy> _enemies = new List<Enemy>(); 
        public List<Enemy> Enemies
        {
            get { return _enemies; }
        }

        [SerializeField]
        private bool _playerIsInRoom;
        public bool PlayerIsInRoom
        {
            get { return _playerIsInRoom; }
            set
            {
                _playerIsInRoom = value;
                _enemies.ForEach(e => e.enabled = true);
            }
        }

        public bool ContainsEnemies { get { return !_enemies.Any(); } }

        public void SetAdjacentRoom(Room room, RoomDirection direction)
        {
            var position = new Vector3();
            var rotation = new Quaternion();
            var door = (Door)Instantiate(room.IsBossRoom ? BossDoorPrefab : DoorPrefab);
            switch (direction)
            {
                case RoomDirection.North:
                    position += new Vector3(0, 4);
                    NorthDoor = door;
                    NorthDoorWall.enabled = false;
                    break;
                case RoomDirection.East:
                    position += new Vector3(7, 0);
                    rotation = Quaternion.Euler(0, 0, 270);
                    EastDoor = door;
                    EastDoorWall.enabled = false;
                    break;
                case RoomDirection.South:
                    position += new Vector3(0, -4);
                    rotation = Quaternion.Euler(0, 0, 180);
                    SouthDoor = door;
                    SouthDoorWall.enabled = false;
                    break;
                case RoomDirection.West:
                    position += new Vector3(-7, 0);
                    rotation = Quaternion.Euler(0, 0, 90);
                    WestDoor = door;
                    WestDoorWall.enabled = false;
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

        public void InstantiateEnemies(Enemy enemyPrefab, Vector2 positionInRoom)
        {
            var enemy = (Enemy) Instantiate(enemyPrefab);
            enemy.transform.position = positionInRoom;
            _enemies.Add(enemy);
            enemy.enabled = false;
        }

        public void OnPlayerEntersRoom(Player player)
        {
            if (ContainsEnemies)
            {
                _doors.ForEach(d => d.IsOpen = false);
            }
            player.CurrentRoom = this;
            Debug.Log("Player entered room: " + name);
        }
    }
}
