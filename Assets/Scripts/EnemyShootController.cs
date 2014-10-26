﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class EnemyShootController : ShootControllerBase
    {
		private Vector2 _shootDirection;
		private bool _shooting;
		private GameObject _player;

		[SerializeField]
		private bool _canShoot = true;
		public bool CanShoot 
		{
			set { _canShoot = value; }
			get { return _canShoot; }
		}
		public override void Start()
		{
			base.Start ();
			_player = GameObject.FindGameObjectWithTag ("Player");
			_shooting = false;
            BulletSpeed = 0.3f;
            ShootingSpeed = 1f;
		}

		public override void Update()
		{
			base.Update ();

			if (CanShootPlayer ()) 
			{
                GetComponent<Animator>().SetBool("Shooting", true);
				StartCoroutine(Shoot());
			}
		}

		IEnumerator Shoot()
		{
			if (!_shooting) {
				_shooting = true;
				var bullet = (Rigidbody2D)Instantiate (BulletPrefab);
				bullet.GetComponent<BulletScript>().Shooter = transform.gameObject;
				bullet.transform.position = transform.position;
				if (_shootDirection.y > 0) {
						bullet.transform.Rotate (0, 0, -90);
				} else if (_shootDirection.y < 0) {
						bullet.transform.Rotate (0, 0, 90);
				} else if (_shootDirection.x > 0) {
						TransformHelpers.FlipX (bullet.gameObject);
				}
				bullet.AddForce (_shootDirection);
			    if (ShootClips.Any())
			    {
			        var clipToPlay = ShootClips[Random.Range(0, ShootClips.Count)];
			        clipToPlay.pitch = Random.Range(MinShootPitch, MaxShootPitch);
                    clipToPlay.Play();
			    }
				yield return new WaitForSeconds (ShootingSpeed*3);
				_shooting = false;
                GetComponent<Animator>().SetBool("Shooting", false);
			}
		}
		
		private bool CanShootPlayer()
		{
			if (_canShoot) 
			{
				_shootDirection = - (transform.position - _player.transform.position);
				//_shootDirection = Vector3.zero - _player.transform.position;
				_shootDirection.Normalize ();
				_shootDirection.Scale (new Vector2 (BulletSpeed, BulletSpeed));
				return true;
			}
			return false;
		}
	}
}
