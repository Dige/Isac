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

        [SerializeField]
        private bool _mirrorAnimation = false;
        public bool MirrorAnimation
        {
            get { return _mirrorAnimation; }
            set { _mirrorAnimation = value; }
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
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100);
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

            SetAnimationDirection(movement, target, currentPosition);
        }

        private void SetAnimationDirection(Vector3 movement, Vector3 target, Vector3 currentPosition)
        {
            int animationDirection = Animator.GetInteger("Direction");
            int newDirection = -1;
            if (movement.x != 0.0f && movement.y != 0.0f)
            {
                newDirection = target.y > currentPosition.y ? 0 : 2;
            }
            else if (movement.x != 0.0f && movement.y == 0.0f)
            {
                newDirection = target.x > currentPosition.x ? 1 : 3;
            }
            else if (movement.x == 0.0f && movement.y != 0.0f)
            {
                newDirection = target.y > currentPosition.y ? 0 : 2;
            }

            if (animationDirection != newDirection)
            {
                Animator.SetInteger("Direction", newDirection);
                if (MirrorAnimation)
                {
                    if ((transform.localScale.x < 0 && newDirection == 3) ||
                        (transform.localScale.x > 0 && newDirection == 1) ||
                        (transform.localScale.y < 0 && newDirection == 0) ||
                        (transform.localScale.y > 0 && newDirection == 2))
                    {
                        FlipX();
                    }
                }
            }
        }

        public void FlipX()
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        public void FlipY()
        {
            Vector3 theScale = transform.localScale;
            theScale.y *= -1;
            transform.localScale = theScale;
        }
    }
}
