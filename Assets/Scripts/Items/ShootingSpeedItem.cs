using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Items
{
    public class ShootingSpeedItem : ItemBase
    {
        public override void UseItem(Player player)
        {
            player.GetComponent<PlayerShootController>().ShootingSpeed += 0.05f;
        }
    }
}
