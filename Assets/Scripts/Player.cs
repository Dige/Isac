using System.Collections;
using System.Linq;
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

    public void OnPickUp(ItemBase item)
    {
        item.enabled = false;
        StartCoroutine(PlayPickUpAnimation(item));
    }

    private IEnumerator PlayPickUpAnimation(ItemBase item)
    {
        item.transform.parent = transform;
        item.transform.localPosition = Vector3.zero + new Vector3(0, 1.0f, 0);
        item.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        item.GetComponent<SpriteRenderer>().sortingOrder = 10;
        Animator.Play("PickUpItem");
        _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Down);
        DisableCharacter();
        yield return new WaitForSeconds(1f);
        EnableCharacter();
        _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Down);
        Destroy(item.gameObject);
        yield return null;
    }

    protected override Vector3 DetermineMovement()
    {
        var movement = new Vector3();

        if (!IsDead && InputHelpers.IsAnyKeyDown("w", "s", "a", "d"))
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

    protected override void Die()
    {
        base.Die();
        Animator.Play("Die");
        DisableCharacter();
    }

    private void DisableCharacter()
    {
        GunObject.enabled = false;
        HeadObject.GetComponent<SpriteRenderer>().enabled = false;
        rigidbody2D.isKinematic = true;
    }

    private void EnableCharacter()
    {
        GunObject.enabled = true;
        HeadObject.GetComponent<SpriteRenderer>().enabled = true;
        rigidbody2D.isKinematic = false;
    }
}
