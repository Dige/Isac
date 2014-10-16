﻿using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;

public class Enemy : CharacterBase
{
    private GameObject _player;

    [SerializeField] 
    private MovementStyle _movementStyle = MovementStyle.TowardsPlayer;
    public MovementStyle MovementStyle
    {
        get { return _movementStyle; }
        set { _movementStyle = value; }
    }

    [SerializeField]
    private float _wanderClipRepeatDelay;
    public float WanderingClipRepeatDelay
    {
        get { return _wanderClipRepeatDelay; }
        set { _wanderClipRepeatDelay = value; }
    }

    [SerializeField]
    private AudioClip _wanderClip;
    public AudioClip WanderingClip
    {
        get { return _wanderClip; }
        set { _wanderClip = value; }
    }

    public override void Start()
    {
        base.Start();
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_wanderClipRepeatDelay > 0.0f)
            StartCoroutine(PlayWanderingClip());
    }

    public IEnumerator PlayWanderingClip()
    {
        audio.PlayOneShot(WanderingClip);
        yield return new WaitForSeconds(WanderingClipRepeatDelay);
    }

    protected override void HandleMovement(Vector3 movement)
    {
        transform.Translate(movement);
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
		// too simple
		var currentPosition = transform.position;
		var moveDirection = -(_player.transform.position - currentPosition);
		moveDirection.Normalize();
		return moveDirection*MoveSpeed;
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