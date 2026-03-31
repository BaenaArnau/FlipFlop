using Godot;
using FlipFlop.scripts.Tools;

namespace FlipFlop.Scripts.Menus
{
	public partial class WinMenu : Control
	{
		[Export] private float _appearDuration = 1.5f;
		private ShaderMaterial _shaderMaterial;
		private float _appearProgress;
		private bool _isAppearing;
		
		[ExportGroup("Buttons")]
		[Export] private Button _againButton;
		[Export] private Button _exitButton;
		
		/// <summary>
		/// Metodo de inicialización del menú de muerte.
		/// Se ejecuta cuando el nodo entra en el árbol de escena.
		/// </summary>
		public override void _Ready()
		{
			SetProcess(false);
			InitializeShader();
		}
		
		/// <summary>
		/// Procesamiento por frame del menú de muerte.
		/// Se ejecuta cada frame después de _PhysicsProcess.
		/// </summary>
		/// <param name="delta"></param>
		public override void _Process(double delta)
		{
			if (_isAppearing && _shaderMaterial != null)
			{
				_appearProgress += (float)delta / _appearDuration;
				
				if (_appearProgress >= 1.0f)
				{
					_appearProgress = 1.0f;
					_isAppearing = false;
				}
				
				_shaderMaterial.SetShaderParameter("progress", _appearProgress);
			}
		}
		
		/// <summary>
		/// Inicializa el ShaderMaterial del ColorRect para la animación de aparición.
		/// </summary>
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
		
		/// <summary>
		/// Cambia la visibilidad del menú de muerte y controla la animación de aparición.
		/// </summary>
		/// <param name="visible">Booleano que nos dice si está visible el objeto o no</param>
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
		
		/// <summary>
		/// Reinicia la escena actual para continuar jugando después de morir.
		/// </summary>
		public void OnAgainPressed()
		{
			ButtonTools.PlayPressAnimation(
				_againButton,
			    () => { GetTree().ChangeSceneToFile("res://scena/Maps/mapa1.tscn"); });
		}
		
		/// <summary>
		/// Regresa al menú principal desde el menú de muerte.
		/// </summary>
		public void OnExitPressed()
		{
			ButtonTools.PlayPressAnimation(
				_exitButton,
				() => { GetTree().ChangeSceneToFile("res://scena/Menus/main_menu.tscn"); });
		}
	}
}
