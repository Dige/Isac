using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class ShootControllerBase : MonoBehaviour
    {
        [SerializeField]
        private float _bulletSpeed = 0.5f;
        public float BulletSpeed
        {
            get { return _bulletSpeed; }
            set { _bulletSpeed = value; }
        }

		[SerializeField]
		private float _shootSpeed = 0.3f;
		public float ShootingSpeed
		{
			get { return _shootSpeed; }
			set { _shootSpeed = value; }
		}

        [SerializeField]
        private Rigidbody2D _bulletPrefab;
        public Rigidbody2D BulletPrefab
        {
            get { return _bulletPrefab; }
            set { _bulletPrefab = value; }
        }

        [SerializeField]
        private List<AudioSource> _shootClips = new List<AudioSource>(1);
        public List<AudioSource> ShootClips
        {
            get { return _shootClips; }
            set { _shootClips = value; }
        }

        public float MinShootPitch = -3.0f;
        public float MaxShootPitch = 3.0f;

        [SerializeField]
        private List<AudioSource> _bulletCollideClips = new List<AudioSource>(1);
        public List<AudioSource> BulletCollideClips
        {
            get { return _bulletCollideClips; }
            set { _bulletCollideClips = value; }
        }
    
        public float MinBulletCollidePitch = -3.0f;
        public float MaxBulletCollidePitch = 3.0f;

		public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }
    }
}
