using UnityEngine;

namespace Assets.Scripts.Items
{
    public class HitAllEnemiesItem : ItemBase
    {
        [SerializeField]
        private int _damage = 2;
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public override void UseItem(Player player)
        {
            foreach (var enemy in player.CurrentRoom.Enemies)
            {
                enemy.Health -= Damage;
            }
        }
    }
}
