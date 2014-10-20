using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _doorOpenClip;
    public AudioSource DoorOpenClip
    {
        get { return _doorOpenClip; }
        set { _doorOpenClip = value; }
    }

    [SerializeField]
    private AudioSource _doorCloseClip;
    public AudioSource DoorCloseClip
    {
        get { return _doorCloseClip; }
        set { _doorCloseClip = value; }
    }

    public Room OwnerRoom { get; set; }

    public RoomDirection Direction { get; set; }

    private Animator _animator;

    private Camera _mainCamera;

	public void Start ()
	{
	    _animator = GetComponent<Animator>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

    private const float PlayerMovement = 2.5f;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetInteger("Is Open", 1);
            if (DoorOpenClip != null)
            {
                DoorOpenClip.Play();
            }

            var cameraMovement = new Vector2();
            var playerMovement = new Vector3();
            switch (Direction)
            {
                case RoomDirection.North:
                    cameraMovement = new Vector2(0, 10);
                    playerMovement = new Vector3(0, PlayerMovement, 0);
                    break;
                case RoomDirection.East:
                    cameraMovement = new Vector2(16, 0);
                    playerMovement = new Vector3(PlayerMovement, 0, 0);
                    break;
                case RoomDirection.South:
                    cameraMovement = new Vector2(0, -10);
                    playerMovement = new Vector3(0, -PlayerMovement, 0);
                    break;
                case RoomDirection.West:
                    cameraMovement = new Vector2(-16, 0);
                    playerMovement = new Vector3(-PlayerMovement, 0, 0);
                    break;
            }

            StartCoroutine(MoveCamera(cameraMovement));
            other.transform.Translate(playerMovement);
        } 
    }

    private IEnumerator MoveCamera(Vector2 direction)
    {
        for (int i = 0; i < 10; i++)
        {
            _mainCamera.transform.Translate(direction.x / 10, direction.y / 10, 0);
            yield return null;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetInteger("Is Open", 0);
            if (DoorCloseClip != null)
            {
                DoorCloseClip.Play();
            }
        }
            
    }
}
