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

        public float MinSpawnPitch = -3.0f;
        public float MaxSpawnPitch = 3.0f;

        [SerializeField]
        private bool _isInstantEffect;
        public bool IsInstantEffect
        {
            get { return _isInstantEffect; }
            set { _isInstantEffect = value; }
        }

        public virtual bool IsInstantlyDestroyedAfterUse { get { return true; } }

        public void Start()
        {
            if (SpawnClip != null)
            {
                SpawnClip.pitch = Random.Range(MinSpawnPitch, MaxSpawnPitch);
                SpawnClip.Play();
            }
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

        public void Enable()
        {
            enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        public void Disable()
        {
            enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }


        private void OnPickUp(Player player)
        {
            if (_pickUpClip != null)
                _pickUpClip.Play();
            player.OnPickUp(this);
        }

        public abstract void UseItem(Player player);
    }
}
