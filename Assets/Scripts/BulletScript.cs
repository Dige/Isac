using System.Linq;
using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BulletScript : MonoBehaviour {

    public float range = 20;

	[SerializeField]
	private GameObject _shooter;
	public GameObject Shooter
	{
		set { _shooter = value; }
		get { return _shooter; }
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject o = collision.gameObject;
        GameObject pc = GameObject.FindWithTag("Player");
		ShootControllerBase shootController = pc.GetComponent<PlayerShootController>();

		if (o.CompareTag("Enemy") && Shooter.Equals(pc))
        {
            o.GetComponentInParent<Enemy>().Health--;
        }
        else if (o.CompareTag("Player") && !Shooter.Equals(pc))
        {
			shootController = Shooter.GetComponent<EnemyShootController>();

			o.GetComponentInParent<Player>().Health--;
        }
        
        if (shootController.BulletCollideClips.Any())
        {
            var clipToPlay = shootController.BulletCollideClips[Random.Range(0, shootController.BulletCollideClips.Count)];
            clipToPlay.pitch = Random.Range(shootController.MinBulletCollidePitch, shootController.MaxBulletCollidePitch);
            clipToPlay.Play();
        }
		Destroy(gameObject);
    }

    private Vector2 _start;

    public void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(RemoveInvulnerability());
		if (Shooter.CompareTag("Enemy")) {
			transform.gameObject.layer = LayerMask.NameToLayer( "Enemy bullet" );
		}
		_start = new Vector2(transform.position.x, transform.position.y);
        var p = GameObject.FindWithTag("Player");
        var pc = p.GetComponent<Player>();
        range = pc.Range;
    }

    private IEnumerator RemoveInvulnerability()
    {
        yield return new WaitForSeconds(0.05f);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private bool _isFading;

    private IEnumerator Fade()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            Color c = GetComponent<SpriteRenderer>().material.color;
            c.a = f;
            renderer.material.color = c;
            yield return null;
        }
    }

	public void Update ()
	{
	    var xDistance = Mathf.Abs(_start.x - transform.position.x);
	    var yDistance = Mathf.Abs(_start.y - transform.position.y);

	    if (!_isFading && (xDistance > range*0.8 || yDistance > range*0.8))
	    {
	        _isFading = true;
	        StartCoroutine(Fade());
	    }

	    if ( xDistance > range || yDistance > range )
	    {
	        Destroy(gameObject);
	    }
	}
}
    