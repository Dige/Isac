using UnityEngine;

public class TargetingController : MonoBehaviour {

	private float _startRotation;

	// Use this for initialization
	void Start ()
	{
	    _startRotation = transform.rotation.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
        {
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;
            target.Normalize();
            float targetAngle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, targetAngle - _startRotation);
        }
	}
}
