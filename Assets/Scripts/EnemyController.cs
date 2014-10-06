using System;
using Assets.Scripts;
using UnityEngine;

public class EnemyController : CharacterControllerBase
{
    private GameObject _player;

    [SerializeField] 
    private MovementStyle _movementStyle = MovementStyle.TowardsPlayer;
    public MovementStyle MovementStyle
    {
        get { return _movementStyle; }
        set { _movementStyle = value; }
    }


    public override void Start()
    {
        base.Start();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void HandleMovement(Vector3 movement)
    {
        transform.position += movement;
    }

    protected override void HandleShooting()
    {

    }

    protected override Vector3 DetermineMovement()
    {
        ShouldMove = true;
        switch (MovementStyle)
        {
            case MovementStyle.TowardsPlayer:
                return MoveTowardsPlayer();
            case MovementStyle.AwayFromPlayer:
                return MoveAwayFromPlayer();
            case MovementStyle.Random:
                return MoveRandomly();
            default:
                throw new NotImplementedException();
        }
    }

    protected override void HandleAnimation(Vector3 movement)
    {
        if (movement == Vector3.zero)
        {
            Animator.enabled = false;
            return;
        }

        Animator.enabled = true;
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            Animator.SetInteger("Direction", movement.x > 0 ? 1 : 3);
        }
        else
        {
            Animator.SetInteger("Direction", movement.y > 0 ? 0 : 2);
        }
    }

    private Vector3 MoveTowardsPlayer()
    {
        var currentPosition = transform.position;
        var moveDirection = _player.transform.position - currentPosition;
        moveDirection.Normalize();
        return moveDirection*MoveSpeed;
    }

    private Vector3 MoveAwayFromPlayer()
    {
        return new Vector3();
    }

    private Vector3 MoveRandomly()
    {
        return new Vector3();
    }
}

public enum MovementStyle
{
    TowardsPlayer,
    AwayFromPlayer,
    Random
}
