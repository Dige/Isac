using UnityEngine;

namespace Assets.Scripts.Items
{
    public class MaxHealthItem : ItemBase
    {
        [SerializeField]
        private int _maxHealthAddition = 2;
        public int MaxHealthAddition
        {
            get { return _maxHealthAddition; }
            set { _maxHealthAddition = value; }
        }


        public override void UseItem(Player player)
        {
            player.MaxHealth += MaxHealthAddition;
        }
    }
}
