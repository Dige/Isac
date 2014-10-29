namespace Assets.Scripts.Items
{
    public class BombPickup : ItemBase
    {
        public override void UseItem(Player player)
        {
            player.GetComponent<PlayerShootController>().NumberofBombs++;
        }
    }
}
