using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2;
    public float minX = 0;
    public float maxX = 100;
    public float minY = 0;
    public float maxY = 100;

    private Vector3 dragOrigin;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;


        var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        var move = new Vector3(-pos.x * dragSpeed, -pos.y * dragSpeed, 0);

        transform.Translate(move, Space.World);
        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
        else if(transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        if(transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
        else if(transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
    }
}
