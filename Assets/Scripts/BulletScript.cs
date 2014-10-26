﻿using System.Linq;
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
        if (o.CompareTag("Enemy"))
        {
            o.GetComponentInParent<Enemy>().Health--;
        }
		GameObject pc = GameObject.FindWithTag ("Player");

        var shootController = pc.GetComponent<PlayerShootController>();
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
		if (Shooter.CompareTag("Enemy")) {
			transform.gameObject.layer = LayerMask.NameToLayer( "Enemy bullet" );
		}
		_start = new Vector2(transform.position.x, transform.position.y);
        var p = GameObject.FindWithTag("Player");
        var pc = p.GetComponent<Player>();
        range = pc.Range;
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
    