using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float range = 20;

	[SerializeField]
	private AudioSource _bulletHitClip;
	public AudioSource BulletHitClip
	{
		get { return _bulletHitClip; }
		set { _bulletHitClip = value; }
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject o = collision.gameObject;
        if (o.CompareTag("Enemy"))
        {
            o.GetComponentInParent<Enemy>().Health--;
        }
		//_bulletHitClip.Play ();
        Destroy(gameObject);
    }
    private Vector2 _start;

    public void Start()
    {
        _start = new Vector2(transform.position.x, transform.position.y);
        GameObject p = GameObject.FindWithTag("Player");
        Player pc = p.GetComponent<Player>();
        range = pc.Range;
    }
	public void Update () {
	    if ( Mathf.Abs(_start.x - transform.position.x) > range || Mathf.Abs(_start.y -transform.position.y) > range )
	    {
	        Destroy(gameObject);
	    }
	}
}
    