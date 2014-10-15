using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class ItemBase : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _spawnClip;
        public AudioClip SpawnClip
        {
            get { return _spawnClip; }
            set { _spawnClip = value; }
        }

        [SerializeField]
        private AudioClip _pickUpClip;
        public AudioClip PickUpClip
        {
            get { return _pickUpClip; }
            set { _pickUpClip = value; }
        }

        protected GameObject Player;

        public virtual void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject o = collision.gameObject;
            if (o.CompareTag("Player"))
            {
                OnPickUp();
                Destroy(gameObject);
            }
        }

        protected virtual void OnPickUp()
        {
            audio.PlayOneShot(_pickUpClip);
        }
    }
}
