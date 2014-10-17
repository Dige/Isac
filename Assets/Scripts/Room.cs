using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Room : MonoBehaviour
    {
        public Room NorthRoom { get; set; }
        public Room SouthRoom { get; set; }
        public Room EastRoom { get; set; }
        public Room WestRoom { get; set; }

        [SerializeField]
        private DoorController _doorPrefab;
        public DoorController DoorPrefab
        {
            get { return _doorPrefab; }
            set { _doorPrefab = value; }
        }

        [SerializeField]
        private DoorController _bossDoorPrefab;
        public DoorController BossDoorPrefab
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


        public void SetAdjacentRoom(Room room, RoomDirection direction)
        {
            var position = new Vector3();
            var rotation = new Quaternion();
            switch (direction)
            {
                case RoomDirection.North:
                    position += new Vector3(0, 4);
                    NorthRoom = room;
                    NorthDoorWall.enabled = false;
                    break;
                case RoomDirection.East:
                    position += new Vector3(7, 0);
                    rotation = Quaternion.Euler(0, 0, 270);
                    EastRoom = room;
                    EastDoorWall.enabled = false;
                    break;
                case RoomDirection.South:
                    position += new Vector3(0, -4);
                    rotation = Quaternion.Euler(0, 0, 180);
                    SouthRoom = room;
                    SouthDoorWall.enabled = false;
                    break;
                case RoomDirection.West:
                    position += new Vector3(-7, 0);
                    rotation = Quaternion.Euler(0, 0, 90);
                    WestRoom = room;
                    WestDoorWall.enabled = false;
                    break;
            }

            var door = (DoorController)Instantiate(room.IsBossRoom ? BossDoorPrefab : DoorPrefab);
            door.Direction = direction;
            door.transform.parent = transform;
            door.transform.localPosition = position;
            door.transform.rotation = rotation;
        }
    }
}
