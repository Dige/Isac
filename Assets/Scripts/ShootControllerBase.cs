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
        private Rigidbody2D _bulletPrefab;
        public Rigidbody2D BulletPrefab
        {
            get { return _bulletPrefab; }
            set { _bulletPrefab = value; }
        }

        [SerializeField]
        private AudioSource _shootClip;
        public AudioSource ShootClip
        {
            get { return _shootClip; }
            set { _shootClip = value; }
        }

        [SerializeField]
        private AudioSource _bulletCollideClip;
        public AudioSource BulletCollideClip
        {
            get { return _bulletCollideClip; }
            set { _bulletCollideClip = value; }
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }
    }
}
