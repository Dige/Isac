using UnityEngine;

namespace Assets.Scripts
{
    public class Room : MonoBehaviour
    {
        public Room NorthRoom { get; set; }

        public Room SouthRoom { get; set; }

        public Room EastRoom { get; set; }

        public Room WestRoom { get; set; }
    }
}
