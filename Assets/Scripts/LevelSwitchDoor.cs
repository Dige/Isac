using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelSwitchDoor : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var floorGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<FloorGenerator>();
            floorGenerator.NumberOfRooms += 2;
            var r = floorGenerator.RoomPrefabs.First();
            r.transform.position = new Vector3(0, -1.1f, 0);
            floorGenerator.FirstRoom = r;
            floorGenerator.ClearFloor();
            floorGenerator.TryGenerateFloor();
            other.transform.position = Vector3.zero;
            floorGenerator.Grid.FirstRoom.OnPlayerEntersRoom(other.GetComponent<Player>());
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(0,0, -10);
        }
    }
}
