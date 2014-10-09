using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float range = 20;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject o = collision.gameObject;
        if (o.CompareTag("Enemy"))
        {
            o.GetComponentInParent<EnemyController>().Health--;
        }
        Destroy(gameObject);
    }
    private Vector2 _start;

    public void Start()
    {
        _start = new Vector2(transform.position.x, transform.position.y);
        GameObject p = GameObject.FindWithTag("Player");
        PlayerController pc = p.GetComponent<PlayerController>();
        range = pc.Range;
    }
	public void Update () {
	    if ( Mathf.Abs(_start.x - transform.position.x) > range || Mathf.Abs(_start.y -transform.position.y) > range )
	    {
	        Destroy(gameObject);
	    }
	}
}
    