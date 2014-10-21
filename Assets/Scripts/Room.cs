using System.Collections;
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
                StartCoroutine(WakeUpEnemies());
            }
        }

        IEnumerator WakeUpEnemies()
        {
            yield return new WaitForSeconds(0.5f);
            _enemies.ForEach(e => e.Enable());
            yield return null;
        }

        public bool ContainsEnemies { get { return _enemies.Count > 0; } }

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

        public void OnEnemyDied(Enemy enemy)
        {
            _enemies.Remove(enemy);
            if (!ContainsEnemies)
            {
                _doors.ForEach(d => d.IsOpen = true);
            }
        }

        public void OnPlayerEntersRoom(Player player)
        {
            PlayerIsInRoom = true;
            if (ContainsEnemies)
            {
                _doors.ForEach(d => d.IsOpen = false);
            }
            else
            {
                _doors.ForEach(d => d.IsOpen = true);
            }
            player.CurrentRoom = this;
            Debug.Log("Player entered room: " + name);
        }
    }
}
