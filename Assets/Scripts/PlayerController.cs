using System.Collections;
using Assets.Scripts;
using UnityEngine;

public class PlayerController : CharacterControllerBase
{   
    private Vector2 _momentum;

    [SerializeField]
    private SpriteRenderer _gunObject;
    public SpriteRenderer GunObject
    {
        get { return _gunObject; }
        set { _gunObject = value; }
    }

    [SerializeField]
    private SpriteRenderer _headObject;
    public SpriteRenderer HeadObject
    {
        get { return _headObject; }
        set { _headObject = value; }
    }

    [SerializeField]
    private Sprite _headFront;
    public Sprite HeadFront
    {
        get { return _headFront; }
        set { _headFront = value; }
    }

    [SerializeField]
    private Sprite _headBack;
    public Sprite HeadBack
    {
        get { return _headBack; }
        set { _headBack = value; }
    }

    [SerializeField]
    private Sprite _headSideways;
    public Sprite HeadSideways
    {
        get { return _headSideways; }
        set { _headSideways = value; }
    }

    [SerializeField]
    private float _shootSpeed = 0.3f;
    public float ShootingSpeed
    {
        get { return _shootSpeed; }
        set { _shootSpeed = value; }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Enemy"))
        {
            Health--;
        }
    }

    private bool _isShooting;
    private KeyCode _shootKey;
    private Vector2 _shootDirection;

    protected override void HandleShooting()
    {
        if (_isShooting)
        {
            SetHeadDirection(_shootKey);
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _shootDirection = new Vector2(0, BulletSpeed);
                _shootKey = KeyCode.UpArrow;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                _shootDirection = new Vector2(0, -BulletSpeed);
                _shootKey = KeyCode.DownArrow;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                _shootDirection = new Vector2(-BulletSpeed, 0);
                _shootKey = KeyCode.LeftArrow;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _shootDirection = new Vector2(BulletSpeed, 0);
                _shootKey = KeyCode.RightArrow;
            }
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        _isShooting = true;
        while (Input.GetKey(_shootKey))
        {
            var bullet = (Rigidbody2D)Instantiate(BulletPrefab);
            bullet.transform.position = transform.position;
            if (_shootDirection.y > 0)
            {
                bullet.transform.Rotate(0, 0, -90);
            }
            else if (_shootDirection.y < 0)
            {
                bullet.transform.Rotate(0, 0, 90);
            }
            else if (_shootDirection.x > 0)
            {
                TransformHelpers.FlipX(bullet.gameObject);
            }
            bullet.AddForce(_shootDirection);
            bullet.AddForce(_momentum * 0.001f);
            yield return new WaitForSeconds(ShootingSpeed);
        }
        _isShooting = false;
    }

    private void SetHeadDirection(KeyCode shootKey)
    {
        switch (shootKey)
        {
            case KeyCode.UpArrow:
                _headObject.sprite = _headBack;
                break;
            case KeyCode.DownArrow:
                _headObject.sprite = _headFront;
                break;
            case KeyCode.LeftArrow:
                _headObject.sprite = _headSideways;
                if ((gameObject.transform.localScale.x < 0 && _headObject.transform.localScale.x > 0) ||
                    gameObject.transform.localScale.x > 0 && _headObject.transform.localScale.x < 0)
                    TransformHelpers.FlipX(_headObject.gameObject);
                break;
            case KeyCode.RightArrow:
                _headObject.sprite = _headSideways;
                if ((gameObject.transform.localScale.x < 0 && _headObject.transform.localScale.x < 0) ||
                    gameObject.transform.localScale.x > 0 && _headObject.transform.localScale.x > 0)
                    TransformHelpers.FlipX(_headObject.gameObject);
                break;
        }
    }

    protected override Vector3 DetermineMovement()
    {
        var movement = new Vector3();

        if (Input.GetKey("w"))
        {
            movement += new Vector3(0, 1, 0);
            ShouldMove = true;
            GunObject.enabled = false;
            if(!_isShooting)
                _headObject.sprite = _headBack;
        }
        if (Input.GetKey("s"))
        {
            movement += new Vector3(0, -1, 0);
            ShouldMove = true;
            GunObject.enabled = true;
            if (!_isShooting)
                _headObject.sprite = _headFront;
        }
        if (Input.GetKey("a"))
        {
            movement += new Vector3(-1, 0, 0);
            ShouldMove = true;
            GunObject.enabled = false;
            if (!_isShooting)
                _headObject.sprite = _headSideways;
        }
        if (Input.GetKey("d"))
        {
            movement += new Vector3(1, 0, 0);
            ShouldMove = true;
            GunObject.enabled = false;
            if (!_isShooting)
                _headObject.sprite = _headSideways;
        }
        return movement;
    }

    protected override void HandleMovement(Vector3 movement)
    {
        _momentum = movement * MoveSpeed;
        gameObject.rigidbody2D.AddForce(_momentum);
    }
}
