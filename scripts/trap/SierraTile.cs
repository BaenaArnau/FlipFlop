using Godot;
using FlipFlop.Scripts.Player;

namespace FlipFlop.Scripts.Trap
{
	/// <summary>
	/// Área de daño para sierras colocadas desde Tiled.
	/// </summary>
	public partial class SierraTile : StaticBody2D
	{
		[Export] Sprite2D _sierraSprite;
		
		public override void _Process(double delta)
		{
			_sierraSprite.Rotation += (float)(delta * 10.0);
		}
		
		private void OnBodyEntered(Node body)
		{
			if (body is Player.Player player)
			{
				player.IsDead = true;
			}
		}
	}
}

