using Godot;
using FlipFlop.Scripts.Player;

namespace FlipFlop.Scripts.Trap
{
	/// <summary>
	/// Área de daño para pinchos colocados desde Tiled.
	/// </summary>
	public partial class PinchoTile : StaticBody2D
	{
		private void OnBodyEntered(Node body)
		{
			if (body is Player.Player player)
			{
				player.IsDead = true;
			}
		}
	}
}

