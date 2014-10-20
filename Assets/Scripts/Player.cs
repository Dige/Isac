using System.Collections;
using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerShootController))]
public class Player : CharacterBase
{
    public Vector2 Momentum { get; private set; }

    [SerializeField]
    private SpriteRenderer _gunObject;
    public SpriteRenderer GunObject
    {
        get { return _gunObject; }
        set { _gunObject = value; }
    }

    [SerializeField]
    private PlayerHeadController _headObject;
    public PlayerHeadController HeadObject
    {
        get { return _headObject; }
        set { _headObject = value; }
    }

    public Room CurrentRoom { get; set; }

    private PlayerShootController _shootController;

    public override void Start()
    {
        base.Start();
        _shootController = GetComponent<PlayerShootController>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Enemy"))
        {
            Health--;
        }
    }

    protected override Vector3 DetermineMovement()
    {
        var movement = new Vector3();

        if (InputHelpers.IsAnyKeyDown("w", "s", "a", "d"))
        {
            ShouldMove = true;
            GunObject.enabled = false;

            var headDirection = PlayerHeadController.HeadDirection.Down;

            if (Input.GetKey("w"))
            {
                movement += new Vector3(0, 1, 0);
                headDirection = PlayerHeadController.HeadDirection.Up;
            }
            else if (Input.GetKey("s"))
            {
                movement += new Vector3(0, -1, 0);
                GunObject.enabled = true;
                headDirection = PlayerHeadController.HeadDirection.Down;
            }
            if (Input.GetKey("a"))
            {
                movement += new Vector3(-1, 0, 0);
                headDirection = PlayerHeadController.HeadDirection.Left;
            }
            else if (Input.GetKey("d"))
            {
                movement += new Vector3(1, 0, 0);
                headDirection = PlayerHeadController.HeadDirection.Right;
            }

            if (!_shootController.IsShooting)
            {
                _headObject.SetHeadDirection(headDirection);
            }       
        }

        return movement;
    }

    protected override void HandleMovement(Vector3 movement)
    {
        Momentum = movement * MoveSpeed;
        gameObject.rigidbody2D.AddForce(Momentum);
    }
}
