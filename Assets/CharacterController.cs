using System.Collections.Generic;
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
    private bool _isSelected;
    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            if (value)
                OnSelected();
            else
                OnUnselected();
        }
    }

    private void OnSelected()
    {
        _isSelected = true;
        renderer.material.color = Color.red;
        _instantiatedTargetingController.SetActive(true);
    }

    private void OnUnselected()
    {
        _isSelected = false;
        renderer.material.color = Color.white;
        _animator.enabled = false;
        _shouldMove = false;
        _instantiatedTargetingController.SetActive(false);
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
    private bool _mouseDown;

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
        // shoot first, move later

        if (!_isSelected)
        {
            return;
        }

        Vector3 currentPosition = transform.position;

        _shouldMove = false;
        _target = currentPosition;
        if (Input.GetKey("w"))
        {
            _target += new Vector3(0, 1, 0);
            _shouldMove = true;
        }
        if (Input.GetKey("s"))
        {
            _target += new Vector3(0, -1, 0);
            _shouldMove = true;
        }
        if (Input.GetKey("a"))
        {
            _target += new Vector3(-1, 0, 0);
            _shouldMove = true;
        }
        if (Input.GetKey("d"))
        {
            _target += new Vector3(1, 0, 0);
            _shouldMove = true;
        }

        if (!_shouldMove)
        {
            _shouldMove = false;
            _animator.enabled = false;
            return;
        }
        else
        {
            _moveDirection = _target - currentPosition;
            _moveDirection.z = _target.z = 0;
            _moveDirection.Normalize();
            _animator.enabled = true;
        }

        
        transform.position = _moveDirection * MoveSpeed * Time.deltaTime + currentPosition;
        if (Mathf.Abs(_target.x - currentPosition.x) > Mathf.Abs(_target.y - currentPosition.y))
        {
            _animator.SetInteger("Direction", _target.x > currentPosition.x ? 1 : 3);
        }
        else
        {
            _animator.SetInteger("Direction", _target.y > currentPosition.y ? 0 : 2);
        }
    }

    public void OnMouseDown()
    {
        var units = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unit"));
        units.ForEach(u => u.GetComponent<CharacterController>().IsSelected = false);
        IsSelected = true;
        _mouseDown = true;
    }

    public void OnMouseUp()
    {
        _mouseDown = false;
    }
}
