using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Player))]
    public class PlayerShootController : ShootControllerBase
    {
        [SerializeField]
        private PlayerHeadController _headObject;
        public PlayerHeadController HeadObject
        {
            get { return _headObject; }
            set { _headObject = value; }
        }

        [SerializeField]
        private float _shootSpeed = 0.3f;
        public float ShootingSpeed
        {
            get { return _shootSpeed; }
            set { _shootSpeed = value; }
        }

        public bool IsShooting { get; private set; }
        private KeyCode _shootKey;
        private Vector2 _shootDirection;

        private Player _player;

        public override void Start()
        {
            base.Start();
            _player = GetComponent<Player>();
        }

        public override void Update()
        {
            base.Update();

            if (IsShooting)
            {
                SetHeadDirection(_shootKey);
                return;
            }

            if (InputHelpers.IsAnyKeyDown(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    _shootDirection = new Vector2(0, BulletSpeed);
                    _shootKey = KeyCode.UpArrow;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    _shootDirection = new Vector2(0, -BulletSpeed);
                    _shootKey = KeyCode.DownArrow;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    _shootDirection = new Vector2(-BulletSpeed, 0);
                    _shootKey = KeyCode.LeftArrow;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    _shootDirection = new Vector2(BulletSpeed, 0);
                    _shootKey = KeyCode.RightArrow;
                }
                StartCoroutine(Shoot());
            }
        }

        IEnumerator Shoot()
        {
            IsShooting = true;
            while (Input.GetKey(_shootKey))
            {
                var bullet = (Rigidbody2D)Instantiate(BulletPrefab);
                bullet.transform.position = transform.position;
                if (_shootDirection.y > 0)
                {
                    bullet.transform.Rotate(0, 0, -90);
                }
                else if (_shootDirection.y < 0)
                {
                    bullet.transform.Rotate(0, 0, 90);
                }
                else if (_shootDirection.x > 0)
                {
                    TransformHelpers.FlipX(bullet.gameObject);
                }
                bullet.AddForce(_shootDirection);
                bullet.AddForce(_player.Momentum * 0.001f);
                ShootClip.Play();
                yield return new WaitForSeconds(ShootingSpeed);
            }
            IsShooting = false;

            //Reset head flipping
            if (_headObject.transform.localScale.x < 0)
            {
                TransformHelpers.FlipX(_headObject.gameObject);
            }
        }

        private void SetHeadDirection(KeyCode shootKey)
        {
            switch (shootKey)
            {
                case KeyCode.UpArrow:
                    _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Up);
                    break;
                case KeyCode.DownArrow:
                    _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Down);
                    break;
                case KeyCode.LeftArrow:
                    _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Left);
                    break;
                case KeyCode.RightArrow:
                    _headObject.SetHeadDirection(PlayerHeadController.HeadDirection.Right);
                    break;
            }
        }
    }
}
