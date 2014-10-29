using System;
using System.Linq;
using System.Collections;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyShootController), typeof(CircleCollider2D))]
public class Enemy : CharacterBase
{
    private GameObject _player;
	private Vector3 _randomDirection;
	private bool _turning = false;
    private bool _jumping = false;
    private bool _inAir = true;

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
    private AudioSource _wanderClip;
    public AudioSource WanderingClip
    {
        get { return _wanderClip; }
        set { _wanderClip = value; }
    }

    public Room OwnerRoom { get; set; }

    public void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_wanderClipRepeatDelay > 0.0f)
            StartCoroutine(PlayWanderingClip());
        if (MovementStyle == MovementStyle.RandomStyle)
        {
            var rnd = (int)(Random.value * 5);
            switch (rnd)
            {
                case 0:
                    MovementStyle = MovementStyle.Stationary;
                    gameObject.GetComponent<EnemyShootController>().CanShoot = true;
                    break;
                case 1:
                    MovementStyle = MovementStyle.RandomDirection;
                    gameObject.GetComponent<EnemyShootController>().CanShoot = false;
                    Flying();
                    break;
                case 2:
                    MovementStyle = MovementStyle.TowardsPlayer;
                    gameObject.GetComponent<EnemyShootController>().CanShoot = false;
                    break;
                case 3:
                    MovementStyle = MovementStyle.RandomTowardsPlayer;
                    gameObject.GetComponent<EnemyShootController>().CanShoot = true;
                    break;
                case 4:
                    MovementStyle = MovementStyle.RandomTowardsPlayer;
                    gameObject.GetComponent<EnemyShootController>().CanShoot = false;
                    break;
                case 5:
                    MovementStyle = MovementStyle.AwayFromPlayer;
                    gameObject.GetComponent<EnemyShootController>().CanShoot = false;
                    break;
            }
        }

    }

    public override void FixedUpdate()
	{
		base.FixedUpdate();
		StartCoroutine (Turn());
	}

    public IEnumerator PlayWanderingClip()
    {
        WanderingClip.Play();
        yield return new WaitForSeconds(WanderingClipRepeatDelay);
    }

    protected override void HandleMovement(Vector3 movement)
    {
        if (Health > 0)
            transform.Translate(movement);
    }

    protected override Vector3 DetermineMovement()
    {
        ShouldMove = true;
        switch (MovementStyle)
        {
            case MovementStyle.JumpToPlayer:
                return JumpToPlayer();
            case MovementStyle.TowardsPlayer:
                return MoveTowardsPlayer();
            case MovementStyle.AwayFromPlayer:
                return MoveAwayFromPlayer();
            case MovementStyle.RandomDirection:
                return MoveRandomly();
			case MovementStyle.RandomTowardsPlayer:
				return WanderTowardsPlayer();
			case MovementStyle.Stationary:
				return Stationary();
            default:
                throw new NotImplementedException();
        }
    }

    protected override void HandleAnimation(Vector3 movement)
    {
        if (MovementStyle != MovementStyle.JumpToPlayer && movement == Vector3.zero)
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

    protected override void TakeDamage()
    {
        base.TakeDamage();
        if (gameObject.GetComponent<EnemyShootController>().Boss)
        {
            OwnerRoom.BossBar.transform.FindChild("BossHealth").transform.localScale = new Vector3((float)Health / MaxHealth,1,1);
        }
        var blood = (GameObject)Instantiate(BloodPrefab.ElementAt(UnityEngine.Random.Range(0,BloodPrefab.Count)));
        blood.transform.position = transform.position;
        blood.transform.parent = OwnerRoom.transform;
    }

    protected override void Die()
    {
        base.Die();
        OwnerRoom.OnEnemyDied(this);
        Disable();
        StartCoroutine(ReallyDie());
    }

    IEnumerator ReallyDie()
    {
        Animator.Play("Die");     
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

	private void Flying()
	{
		transform.gameObject.layer = LayerMask.NameToLayer ("Flying enemy");
	}

    private Vector3 JumpToPlayer()
    {
        if (_jumping && !_inAir)
        {
            _jumping = false;
            StartCoroutine(Jump());
            return Vector3.zero; // currentPosition - playerPosition;
        }
        else if (!_jumping && _inAir)
        {
            _inAir = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;

            StartCoroutine(Wait());
            return Vector3.zero;
        }
        return Vector3.zero;
    }

    IEnumerator Jump()
    {
        Animator.SetBool("Jumping", true);
        
        yield return new WaitForSeconds(0.35f);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.6f);
        transform.position = _player.transform.position + new Vector3(0.2f, 0, 0);
        yield return new WaitForSeconds(0.6f);
        _inAir = true;
        Animator.SetBool("Jumping", false);
    }

    IEnumerator Wait()
    {
        if (gameObject.GetComponent<EnemyShootController>().Boss)
            gameObject.GetComponent<EnemyShootController>().BossExplode();

        if (gameObject.GetComponent<EnemyShootController>().Boss && UnityEngine.Random.Range(0, 2) < 1 && (_player.transform.position - transform.position).magnitude < 7)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2, 3));
            gameObject.GetComponent<EnemyShootController>().BossShoot();
            //_inAir = true;
            _jumping = true;

        }
        else if (gameObject.GetComponent<EnemyShootController>().Boss)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1, 2));
            _jumping = true;
        }
        else
        {

            yield return new WaitForSeconds(UnityEngine.Random.Range(2, 4));
            _jumping = true;
        }
    }


    private Vector3 MoveTowardsPlayer()
    {
        // could use some pathing, boo no navigationmesh for 2D
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
		var moveDirection = _randomDirection;
		moveDirection.Normalize();
		return moveDirection * MoveSpeed;
    }

	private Vector3 WanderTowardsPlayer()
	{
		var currentPosition = transform.position;
		var moveDirection = _player.transform.position - currentPosition;
		moveDirection += _randomDirection;
		moveDirection.Normalize();
		return moveDirection*MoveSpeed;
	}

	IEnumerator Turn()
	{
		if (!_turning) 
		{
			_turning = true;
			Vector2 direction = UnityEngine.Random.insideUnitCircle * 4f;
			_randomDirection = new Vector3 (direction.x, direction.y, 0);
			yield return new WaitForSeconds (0.5f);
            _turning = false;
        }
	}

	public void Enable()
	{
		GetComponents<MonoBehaviour>().ToList().ForEach(e => e.enabled = true);
	}

	public void Disable(Room room = null)
	{
		if (room != null)
			OwnerRoom =room;
		GetComponents<MonoBehaviour>().ToList().ForEach(e => e.enabled = false);
	}

	private Vector3 Stationary()
	{
		return new Vector3 ();
	}
}

public enum MovementStyle
{
    TowardsPlayer,
    AwayFromPlayer,
    RandomDirection,
	RandomTowardsPlayer,
	Stationary,
    RandomStyle,
    JumpToPlayer
}
