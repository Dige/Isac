using UnityEngine;

namespace Assets.Scripts.Items
{
	public class MovementSpeedItem : ItemBase
	{
		[SerializeField]
		private float _movementSpeedAddition = 0.5f;
		public float MovementSpeedAddition
		{
			get { return _movementSpeedAddition; }
			set { _movementSpeedAddition = value; }
		}
		
		public override void UseItem(Player player)
		{
			player.GetComponent<Player>().MoveSpeed += MovementSpeedAddition;
		}
	}
}

