using UnityEngine;

namespace Assets.Scripts
{
    public abstract class ItemBase : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _spawnClip;
        public AudioSource SpawnClip
        {
            get { return _spawnClip; }
            set { _spawnClip = value; }
        }

        [SerializeField]
        private AudioSource _pickUpClip;
        public AudioSource PickUpClip
        {
            get { return _pickUpClip; }
            set { _pickUpClip = value; }
        }

        [SerializeField]
        private bool _isInstantEffect;
        public bool IsInstantEffect
        {
            get { return _isInstantEffect; }
            set { _isInstantEffect = value; }
        }
        

        public void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject o = collision.gameObject;
            if (o.CompareTag("Player"))
            {
                OnPickUp(o.GetComponent<Player>());
                //Destroy(gameObject);
            }
        }

        private void OnPickUp(Player player)
        {
            player.OnPickUp(this);
            if (_pickUpClip != null)
				_pickUpClip.Play();
        }

        public abstract void UseItem(Player player);
    }
}
