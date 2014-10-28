﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(CircleCollider2D), typeof(SpriteRenderer))]
    public class Bomb : MonoBehaviour
    {
        private readonly List<Enemy> _enemies = new List<Enemy>();
        private readonly List<Tile> _tiles = new List<Tile>(); 

        public void Start()
        {
            StartCoroutine(Detonate());
        }

        private IEnumerator Detonate()
        {
            yield return new WaitForSeconds(1.5f);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Tile>() != null)
            {
                Debug.Log("Tile inside");
                _tiles.Add(other.GetComponent<Tile>());
            }
            else if (other.tag.Equals("Enemy"))
            {
                Debug.Log("Enemy Inside");
                _enemies.Add(other.GetComponent<Enemy>());
            }

        }

        public void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<Tile>() != null)
            {
                Debug.Log("Tile inside");
            }
            else if (other.tag.Equals("Enemy"))
            {
                Debug.Log("Enemy Inside");
            }

        }

        public void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Tile>() != null)
            {
                Debug.Log("Tile exit");
                _tiles.Remove(other.GetComponent<Tile>());
            }
            else if (other.tag.Equals("Enemy"))
            {
                Debug.Log("Enemy exit");
                _enemies.Remove(other.GetComponent<Enemy>());
            }

        }
    }
}
