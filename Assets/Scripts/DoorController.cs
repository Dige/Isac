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

    private Animator _animator;

	public void Start ()
	{
	    _animator = GetComponent<Animator>();
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetInteger("Is Open", 1);
            if (DoorOpenClip != null)
            {
                DoorOpenClip.Play();
            }
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
