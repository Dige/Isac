using UnityEngine;
using System.Collections;

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
    private float _turnSpeed;
    public float TurnSpeed
    {
        get { return _turnSpeed; }
        set { _turnSpeed = value; }
    }

    private Vector3 _moveDirection;
    private Vector3 _target;
    private bool _shouldMove;
    private Animator _animator;

	public void Start ()
	{
	    _moveDirection = Vector3.down;
        _animator = this.GetComponent<Animator>();
		_animator.enabled = false;
	}
	
	public void Update () {

        Vector3 currentPosition = transform.position;
        if (Input.GetButton("Fire1"))
        {
            _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _moveDirection = _target - currentPosition;
            _moveDirection.z = _target.z = 0;
            _moveDirection.Normalize();
            _shouldMove = true;
            _animator.enabled = true;
        }

        if (!_shouldMove)
        {
            return;
        }
           
        Vector3 target = _moveDirection * MoveSpeed + currentPosition;
        transform.position = currentPosition = Vector3.Lerp(currentPosition, target, Time.deltaTime);
        if (Mathf.Abs(_target.x - currentPosition.x) > Mathf.Abs(_target.y - currentPosition.y))
        {
            _animator.SetInteger("Direction", _target.x > currentPosition.x ? 1 : 3);
        }
        else
        {
            _animator.SetInteger("Direction", _target.y > currentPosition.y ? 0 : 2);
        }

        if(Vector3.Distance(transform.position, _target) < 0.1)
        {
            _shouldMove = false;
            _animator.enabled = false;
        }
	}
}
