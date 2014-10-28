using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> _destroyedSprites;
        public List<Sprite> DestroyedSprites
        {
            get { return _destroyedSprites; }
        }

        public void Destroy()
        {
            Destroy(GetComponent<BoxCollider2D>());
            GetComponent<SpriteRenderer>().sprite = DestroyedSprites[Random.Range(0, DestroyedSprites.Count)];
        }
    }
}
