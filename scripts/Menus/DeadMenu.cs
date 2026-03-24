using Godot;
using System;
using FlipFlop.scripts.Tools;

namespace FlipFlop.Scripts.Menus
{
	public partial class DeadMenu : Control
	{
		[Export] private NodePath _deathMessageLabelPath;
		private Label _deathMessageLabel;
		[Export] private float AppearDuration = 1.5f;
		private const string CSV_PATH = "res://files/DeadMensaje.csv";
		private ShaderMaterial _shaderMaterial;
		private float _appearProgress = 0.0f;
		private bool _isAppearing = false;
		
		[ExportGroup("Buttons")]
		[Export] private Button _continueButton;
		[Export] private Button _exitButton;
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			SetProcess(false);
			_deathMessageLabel = GetNodeOrNull<Label>(_deathMessageLabelPath);
			LoadRandomDeathMessage();
			InitializeShader();
		}

		public override void _Process(double delta)
		{
			if (_isAppearing && _shaderMaterial != null)
			{
				_appearProgress += (float)delta / AppearDuration;
				
				if (_appearProgress >= 1.0f)
				{
					_appearProgress = 1.0f;
					_isAppearing = false;
				}
				
				_shaderMaterial.SetShaderParameter("progress", _appearProgress);
			}
		}

		private void InitializeShader()
		{
			// Obtener el ColorRect hijo
			var colorRect = GetNode<ColorRect>("ColorRect");
			
			if (colorRect != null && colorRect.Material is ShaderMaterial shaderMat)
			{
				_shaderMaterial = shaderMat;
				_shaderMaterial.SetShaderParameter("progress", 0.0f);
				GD.Print("✓ ShaderMaterial listo en ColorRect");
			}
			else
			{
				GD.PrintErr("✗ Error: No se encontró el ShaderMaterial en ColorRect");
			}
		}

		private void LoadRandomDeathMessage()
		{
			if (_deathMessageLabel == null)
			{
				GD.PrintErr("✗ Error: Label de mensaje de muerte no asignada en la escena");
				return;
			}

			var file = FileAccess.Open(CSV_PATH, FileAccess.ModeFlags.Read);
			if (file == null)
			{
				GD.PrintErr($"No se pudo abrir el archivo: {CSV_PATH}");
				return;
			}

			var messages = new System.Collections.Generic.List<string>();
			bool isFirstLine = true;

			while (!file.EofReached())
			{
				var line = file.GetLine().Trim();
				
				// Saltar línea de encabezado y líneas vacías
				if (isFirstLine || string.IsNullOrEmpty(line))
				{
					isFirstLine = false;
					continue;
				}

				// Extraer el mensaje (todo lo que viene después de la primera coma)
				var parts = line.Split(',', 2);
				if (parts.Length >= 2)
				{
					var message = parts[1].Trim();
					messages.Add(message);
				}
			}

			if (messages.Count > 0)
			{
				var random = new RandomNumberGenerator();
				int randomIndex = (int)(random.Randi() % messages.Count);
				_deathMessageLabel.Text = messages[randomIndex];
			}
		}
		
		public void ChangeVisibility(bool visible)
		{
			Visible = visible;
			
			if (visible)
			{
				// Iniciar la animación de aparición
				_appearProgress = 0.0f;
				_isAppearing = true;
				SetProcess(true);
			}
			else
			{
				// Detener la animación y resetear
				_isAppearing = false;
				_appearProgress = 0.0f;
				SetProcess(false);
				if (_shaderMaterial != null)
				{
					_shaderMaterial.SetShaderParameter("progress", 0.0f);
				}
			}
		}
		
		public void OnContinuePressed()
		{
			ButtonTools.PlayPressAnimation(
				_continueButton,
				() => { GetTree().ReloadCurrentScene(); });
		}

		public void OnExitPressed()
		{
			ButtonTools.PlayPressAnimation(
				_exitButton,
				() => { GetTree().ChangeSceneToFile("res://scena/Menus/main_menu.tscn"); });
		}
	}
}
