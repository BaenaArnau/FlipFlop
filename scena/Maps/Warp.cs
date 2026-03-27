using FlipFlop.Scripts.Player;
using Godot;
using System;

namespace FlipFlop.Scripts.Warp{
public partial class Warp : Area2D
{
	private string currentScene;
	private string nextScene;
	
	public override void _Ready()
	{
		// Obtener el nombre del archivo de la escena actual
		string scenePath = GetTree().CurrentScene.SceneFilePath;
		currentScene = scenePath.GetFile().GetBaseName();
		
		// Calcular la siguiente escena
		int currentLevel = int.Parse(currentScene.Replace("mapa", ""));
		nextScene = "mapa" + (currentLevel + 1).ToString();
	}

	public override void _Process(double delta)
	{
	}

	public void OnBodyEntered(Node body)
	{
		if (body is Player.Player)
		{
            GD.Print("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
			CallDeferred(MethodName.ChangeScene);
		}
	}
	
	private void ChangeScene()
	{
		GetTree().ChangeSceneToFile("res://scena/Maps/" + nextScene + ".tscn");
	}
}
}