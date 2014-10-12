using UnityEngine;

namespace Assets.Scripts
{
    public abstract class ItemBase : MonoBehaviour
    {
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

        protected abstract void OnPickUp();
    }
}
