using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    private Vector3 _moveDirection;
    private Vector3 _target;
    private bool _shouldMove;

    private Animator _animator;

    [SerializeField]
    private Rigidbody2D _bulletPrefab;
    public Rigidbody2D BulletPrefab
    {
        get { return _bulletPrefab; }
        set { _bulletPrefab = value; }
    }


    public void Start()
    {
        _moveDirection = Vector3.down;
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    public void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)
            || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            var bullet = (Rigidbody2D)Instantiate(_bulletPrefab);
            bullet.transform.position = transform.position;
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                bullet.transform.Rotate(0, 0, -90);
                bullet.AddForce(new Vector2(0, 1));
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                bullet.transform.Rotate(0, 0, 90);
                bullet.AddForce(new Vector2(0, -1));
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                bullet.AddForce(new Vector2(-1, 0));
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                bullet.transform.Rotate(0, 0, -180);
                bullet.AddForce(new Vector2(1, 0));
            }
        }
    }

    private const float ANIMATION_STOP_VELOCITY = 0.35f;

    private void HandleMovement()
    {
        Vector3 currentPosition = transform.position;

        _shouldMove = false;
        _target = currentPosition;

        var movement = new Vector3();

        if (Input.GetKey("w"))
        {
            movement += new Vector3(0, 1, 0);
            _shouldMove = true;
        }
        if (Input.GetKey("s"))
        {
            movement += new Vector3(0, -1, 0);
            _shouldMove = true;
        }
        if (Input.GetKey("a"))
        {
            movement += new Vector3(-1, 0, 0);
            _shouldMove = true;
        }
        if (Input.GetKey("d"))
        {
            movement += new Vector3(1, 0, 0);
            _shouldMove = true;
        }

        if (_animator.enabled && gameObject.rigidbody2D.velocity.x < ANIMATION_STOP_VELOCITY && gameObject.rigidbody2D.velocity.x > -ANIMATION_STOP_VELOCITY
            && gameObject.rigidbody2D.velocity.y < ANIMATION_STOP_VELOCITY && gameObject.rigidbody2D.velocity.y > -ANIMATION_STOP_VELOCITY)
        {
            _animator.enabled = false;
            return;
        }

        if (!_shouldMove)
            return;

        _target += movement;
        _moveDirection = _target - currentPosition;
        _moveDirection.z = _target.z = 0;
        _moveDirection.Normalize();
        _animator.enabled = true;

        //Vector3 target = _moveDirection * MoveSpeed + currentPosition;
        //transform.position = currentPosition = Vector3.Lerp(currentPosition, target, Time.deltaTime);
        gameObject.rigidbody2D.AddForce(movement*MoveSpeed);
        if (movement.x != 0.0f && movement.y != 0.0f)
        {
            _animator.SetInteger("Direction", _target.y > currentPosition.y ? 0 : 2);
        }
        else if (movement.x != 0.0f && movement.y == 0.0f)
        {
            _animator.SetInteger("Direction", _target.x > currentPosition.x ? 1 : 3);
        }
        else if (movement.x == 0.0f && movement.y != 0.0f)
        {
            _animator.SetInteger("Direction", _target.y > currentPosition.y ? 0 : 2);
        }
    }
}
