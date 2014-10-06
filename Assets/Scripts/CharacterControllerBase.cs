using UnityEngine;

namespace Assets.Scripts
{
    public abstract class CharacterControllerBase : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed;
        public float MoveSpeed
        {
            get { return _moveSpeed; }
            set { _moveSpeed = value; }
        }

        [SerializeField]
        private float _bulletSpeed = 0.5f;
        public float BulletSpeed
        {
            get { return _bulletSpeed; }
            set { _bulletSpeed = value; }
        }

        protected bool ShouldMove;

        protected Animator Animator;

        [SerializeField]
        private Rigidbody2D _bulletPrefab;
        public Rigidbody2D BulletPrefab
        {
            get { return _bulletPrefab; }
            set { _bulletPrefab = value; }
        }

        [SerializeField]
        private int _health = 8;
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        [SerializeField]
        private int _range = 8;
        public int Range
        {
            get { return _range; }
            set { _range = value; }
        }

        public virtual void Start()
        {
            Animator = GetComponent<Animator>();
            Animator.enabled = false;
        }

        public virtual void Update()
        {
            if ( Health <= 0 && gameObject.tag == "Enemy")
            {
                Destroy(gameObject);
                
            }
            var movement = DetermineMovement();
            HandleAnimation(movement);
            HandleMovement(movement);
            HandleShooting();
        }

        protected abstract void HandleMovement(Vector3 movement);

        protected abstract void HandleShooting();
        protected abstract Vector3 DetermineMovement();

        private const float ANIMATION_STOP_VELOCITY = 0.35f;

        protected virtual void HandleAnimation(Vector3 movement)
        {
            Vector3 currentPosition = transform.position;

            if (Animator.enabled && gameObject.rigidbody2D.velocity.x < ANIMATION_STOP_VELOCITY && gameObject.rigidbody2D.velocity.x > -ANIMATION_STOP_VELOCITY
                && gameObject.rigidbody2D.velocity.y < ANIMATION_STOP_VELOCITY && gameObject.rigidbody2D.velocity.y > -ANIMATION_STOP_VELOCITY)
            {
                Animator.enabled = false;
                return;
            }

            if (!ShouldMove)
                return;

            var target = currentPosition + movement;
            Animator.enabled = true;

            if (movement.x != 0.0f && movement.y != 0.0f)
            {
                Animator.SetInteger("Direction", target.y > currentPosition.y ? 0 : 2);
            }
            else if (movement.x != 0.0f && movement.y == 0.0f)
            {
                Animator.SetInteger("Direction", target.x > currentPosition.x ? 1 : 3);
            }
            else if (movement.x == 0.0f && movement.y != 0.0f)
            {
                Animator.SetInteger("Direction", target.y > currentPosition.y ? 0 : 2);
            }
        }
    }
}
