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

        public void SetAdjacentRoom(Room room, RoomDirection direction)
        {
            var position = new Vector3();
            var rotation = new Quaternion();
            switch (direction)
            {
                case RoomDirection.North:
                    position += new Vector3(0, 4);
                    NorthRoom = room;
                    break;
                case RoomDirection.East:
                    position += new Vector3(7, 0);
                    rotation = Quaternion.Euler(0, 0, 270);
                    EastRoom = room;
                    break;
                case RoomDirection.South:
                    position += new Vector3(0, -4);
                    rotation = Quaternion.Euler(0, 0, 180);
                    SouthRoom = room;
                    break;
                case RoomDirection.West:
                    position += new Vector3(-7, 0);
                    rotation = Quaternion.Euler(0, 0, 90);
                    WestRoom = room;
                    break;
            }

            var door = (DoorController)Instantiate(room.IsBossRoom ? BossDoorPrefab : DoorPrefab);
            door.transform.parent = transform;
            door.transform.localPosition = position;
            door.transform.rotation = rotation;
        }
    }
}
