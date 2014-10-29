using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(CircleCollider2D), typeof(SpriteRenderer), typeof(Animator))]
    public class Bomb : MonoBehaviour
    {
        [SerializeField]
        private int _damage = 4;
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        [SerializeField]
        private float _detonationTime = 1.5f;
        public float DetonationTime
        {
            get { return _detonationTime; }
            set { _detonationTime = value; }
        }
        

        public AudioSource ExplosionClip;

        private readonly List<Enemy> _enemies = new List<Enemy>();
        private readonly List<Tile> _tiles = new List<Tile>(); 

        public void Start()
        {
            StartCoroutine(Detonate());
        }

        private IEnumerator Detonate()
        {
            GetComponent<Animator>().SetBool("Explode", true);
            yield return new WaitForSeconds(DetonationTime);
            if(ExplosionClip != null)
                ExplosionClip.Play();
            _enemies.ForEach(e => e.Health -= Damage);
            _tiles.ForEach(t => t.Destroy());
            Destroy(gameObject);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Tile>() != null)
            {
                _tiles.Add(other.GetComponent<Tile>());
            }
            else if (other.tag.Equals("Enemy"))
            {
                _enemies.Add(other.GetComponent<Enemy>());
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<Tile>() != null)
            {
                _tiles.Remove(other.GetComponent<Tile>());
            }
            else if (other.tag.Equals("Enemy"))
            {
                _enemies.Remove(other.GetComponent<Enemy>());
            }
        }
    }
}
