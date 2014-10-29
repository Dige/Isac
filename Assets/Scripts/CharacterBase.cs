using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public abstract class CharacterBase : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed;
        public float MoveSpeed
        {
            get { return _moveSpeed; }
            set { _moveSpeed = value; }
        }

        [SerializeField]
        private List<GameObject> _bloodPrefab;
        public List<GameObject> BloodPrefab
        {
            set { _bloodPrefab = value; }
            get { return _bloodPrefab;  }
        }

        protected bool ShouldMove;

        protected Animator Animator;

        protected SpriteRenderer SpriteRenderer;

        [SerializeField]
        private int _maxHealth;
        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }

        [SerializeField]
        private int _health = 8;
        public int Health
        {
            get { return _health; }
            set
            {
                if (value < _health)
                {
                    TakeDamage();
                }
                _health = value;
                if (_health <= 0)
                {
                    Die();
                }
            }
        }

        public bool IsDead { get { return _health <= 0; } }

        [SerializeField]
        private int _range = 8;
        public int Range
        {
            get { return _range; }
            set { _range = value; }
        }

        [SerializeField]
        private List<AudioSource> _takeDamageClips = new List<AudioSource>(1);
        public List<AudioSource> TakeDamageClips
        {
            get { return _takeDamageClips; }
        }

        public float MinDamagedPitch = -3.0f;
        public float MaxDamagedPitch = 3.0f;

        [SerializeField]
        private AudioSource _dieClip;
        public AudioSource DieClip
        {
            get { return _dieClip; }
            set { _dieClip = value; }
        }

        public float MinDiePitch = -3.0f;
        public float MaxDiePitch = 3.0f;

        [SerializeField]
        private bool _mirrorAnimation;
        public bool MirrorAnimation
        {
            get { return _mirrorAnimation; }
            set { _mirrorAnimation = value; }
        }

        public virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator.enabled = false;
        }

        public virtual void FixedUpdate()
        {
            var movement = DetermineMovement();
            HandleAnimation(movement);
            HandleMovement(movement);
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100);
        }

        protected abstract void HandleMovement(Vector3 movement);

        protected abstract Vector3 DetermineMovement();

        private const float ANIMATION_STOP_VELOCITY = 0.35f;

        protected virtual void HandleAnimation(Vector3 movement)
        {
            Vector3 currentPosition = transform.position;

            if (Animator.enabled 
			    && gameObject.rigidbody2D.velocity.x < ANIMATION_STOP_VELOCITY 
			    && gameObject.rigidbody2D.velocity.x > -ANIMATION_STOP_VELOCITY
                && gameObject.rigidbody2D.velocity.y < ANIMATION_STOP_VELOCITY 
			    && gameObject.rigidbody2D.velocity.y > -ANIMATION_STOP_VELOCITY)
            {
                Animator.enabled = false;
                ShouldMove = false;
                return;
            }

            if (!ShouldMove)
                return;

            var target = currentPosition + movement;
            Animator.enabled = true;

            SetAnimationDirection(movement, target, currentPosition);
        }



        protected virtual void TakeDamage()
        {
            StartCoroutine(DamageBlink());
            if (_takeDamageClips.Any())
            {
                var clipToPlay = _takeDamageClips[Random.Range(0, _takeDamageClips.Count)];
                clipToPlay.pitch = Random.Range(MinDamagedPitch, MaxDamagedPitch);
                clipToPlay.Play();
            }
				
        }

        private IEnumerator DamageBlink()
        {
            Color c = gameObject.GetComponent<SpriteRenderer>().color;
            ChangeSpriteColorRecursively(Color.red);
            yield return new WaitForSeconds(0.1f);
            ChangeSpriteColorRecursively(c);
        }

        private void ChangeSpriteColorRecursively(Color color)
        {
            SpriteRenderer.color = color;
            foreach (var childrenSprite in GetComponentsInChildren<SpriteRenderer>())
            {
                childrenSprite.color = color;
            }
        }

        protected virtual void Die()
        {
            if (_dieClip != null)
            {
                _dieClip.pitch = Random.Range(MinDiePitch, MaxDiePitch);
                _dieClip.Play();
            }
				
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
                        TransformHelpers.FlipX(gameObject);
                    }
                }
            }
        }
    }
}
