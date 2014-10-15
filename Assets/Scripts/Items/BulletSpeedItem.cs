using UnityEngine;

namespace Assets.Scripts.Items
{
    public class BulletSpeedItem : ItemBase
    {
        [SerializeField]
        private float _bulletSpeedAddition = 0.5f;
        public float BulletSpeedAddition
        {
            get { return _bulletSpeedAddition; }
            set { _bulletSpeedAddition = value; }
        }

        protected override void OnPickUp()
        {
            Player.GetComponent<PlayerShootController>().BulletSpeed += BulletSpeedAddition;
        }
    }
}
