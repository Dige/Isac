using UnityEngine;

public class BulletScript : MonoBehaviour {

	public void Update () {
	    if (transform.position.x > 20 || transform.position.x < -20
	        || transform.position.y > 20 || transform.position.y < -20)
	    {
	        Destroy(gameObject);
	    }
	}
}
