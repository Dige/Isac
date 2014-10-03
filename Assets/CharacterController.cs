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

    [SerializeField]
    private GameObject _targetingController;
    public GameObject TargetingController
    {
        get { return _targetingController; }
        set { _targetingController = value; }
    }

    private GameObject _instantiatedTargetingController;

    private Vector3 _moveDirection;
    private Vector3 _target;
    private bool _shouldMove;

    private Animator _animator;


    public void Start()
    {
        _moveDirection = Vector3.down;
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        _instantiatedTargetingController = Instantiate(_targetingController) as GameObject;
        _instantiatedTargetingController.transform.parent = transform;
        _instantiatedTargetingController.transform.localPosition = Vector3.zero;
        _instantiatedTargetingController.SetActive(false);
    }

    public void Update()
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

        if (!_shouldMove)
        {
            _animator.enabled = false;
            return;
        }
        _target += movement;
        _moveDirection = _target - currentPosition;
        _moveDirection.z = _target.z = 0;
        _moveDirection.Normalize();
        _animator.enabled = true;

        Vector3 target = _moveDirection * MoveSpeed + currentPosition;
        transform.position = currentPosition = Vector3.Lerp(currentPosition, target, Time.deltaTime);
        if (movement.x != 0.0f && movement.y != 0.0f)
        {
            _animator.SetInteger("Direction", _target.y > currentPosition.y ? 0 : 2);
        }
        else if (Mathf.Abs(_target.x - currentPosition.x) > Mathf.Abs(_target.y - currentPosition.y))
        {
            _animator.SetInteger("Direction", _target.x > currentPosition.x ? 1 : 3);
        }
        else if(Mathf.Abs(_target.x - currentPosition.x) > Mathf.Abs(_target.y - currentPosition.y))
        {
            _animator.SetInteger("Direction", _target.y > currentPosition.y ? 0 : 2);
        }
    }
}
