using Assets.Scripts;
using UnityEngine;

public class CharacterController : CharacterControllerBase
{   
    private Vector2 _momentum;

    protected override void HandleShooting()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)
            || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            var bullet = (Rigidbody2D)Instantiate(BulletPrefab);
            bullet.transform.position = transform.position;
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                bullet.transform.Rotate(0, 0, -90);
                bullet.AddForce(new Vector2(0, BulletSpeed));
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                bullet.transform.Rotate(0, 0, 90);
                bullet.AddForce(new Vector2(0, -BulletSpeed));
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                bullet.AddForce(new Vector2(-BulletSpeed, 0));
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                bullet.transform.Rotate(0, 0, -180);
                bullet.AddForce(new Vector2(BulletSpeed, 0));
            }
            bullet.AddForce(_momentum*0.001f);
        }
    }

    protected override Vector3 DetermineMovement()
    {
        var movement = new Vector3();

        if (Input.GetKey("w"))
        {
            movement += new Vector3(0, 1, 0);
            ShouldMove = true;
        }
        if (Input.GetKey("s"))
        {
            movement += new Vector3(0, -1, 0);
            ShouldMove = true;
        }
        if (Input.GetKey("a"))
        {
            movement += new Vector3(-1, 0, 0);
            ShouldMove = true;
        }
        if (Input.GetKey("d"))
        {
            movement += new Vector3(1, 0, 0);
            ShouldMove = true;
        }
        return movement;
    }

    protected override void HandleMovement(Vector3 movement)
    {
        _momentum = movement * MoveSpeed;
        gameObject.rigidbody2D.AddForce(_momentum);
    }
}
