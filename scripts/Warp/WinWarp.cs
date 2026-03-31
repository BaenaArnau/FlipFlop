using FlipFlop.Scripts.Menus;
using Godot;
using System;

namespace FlipFlop.Scripts.Warp{
public partial class WinWarp : Area2D
{
	private string currentScene;
	private string nextScene;
	[Export] private WinMenu _winMenu;

	public override void _Process(double delta)
	{
	}

	public void OnBodyEntered(Node body)
{
    if (body is Player.Player player)
    {
        // Sacar la cámara del Player y moverla al root de la escena
        var camera = player.GetNodeOrNull<Camera2D>("Camera2D");
        if (camera != null)
        {
            var sceneRoot = GetTree().CurrentScene;
            camera.Reparent(sceneRoot);
            camera.Position = Vector2.Zero; // Centrar en origen (donde está el WinMenu)
        }
        _winMenu?.ChangeVisibility(true);
        QueueFree();
    }
}
	
}
}