using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class FlyItem : ItemBase
    {
        public override bool IsInstantlyDestroyedAfterUse
        {
            get { return false; }
        }

        public override void UseItem(Player player)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "Player";
            spriteRenderer.sortingOrder = -1;
            transform.localPosition = Vector3.zero + new Vector3(0, 1.5f, 0);
            spriteRenderer.enabled = true;

            StartCoroutine(Fly(player));
        }

        private IEnumerator Fly(Player player)
        {
            player.gameObject.layer = 19;
            var currentRoom = player.CurrentRoom;
            while (player.CurrentRoom == currentRoom)
            {
                yield return null;
            }
            Destroy(gameObject);
            player.gameObject.layer = 9;
        }
    }
}
