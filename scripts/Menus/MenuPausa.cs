using Godot;
using FlipFlop.scripts.Tools;

namespace FlipFlop.Scripts.Menus
{
	/// <summary>
	/// Controlador del menú de pausa.
	/// Gestiona la visibilidad del menú y permite pausar el juego solo si el jugador no se ha movido.
	/// </summary>
	public partial class MenuPausa : Control
	{
		/// <summary>
		/// Referencia al menú de opciones
		/// </summary>
		[Export] private Control _optionsMenu;

		/// <summary>
		/// Botón para reanudar el juego
		/// </summary>
		[Export] private Button _returnButton;

		/// <summary>
		/// Botón para salir del juego
		/// </summary>
		[Export] private Button _exitButton;

		/// <summary>
		/// Botón para abrir el menú de configuración
		/// </summary>
		[Export] private Button _settingsButton;

	/// <summary>
	/// Referencia al nodo del jugador para validar si ha realizado movimiento
	/// </summary>
	[Export] private Player.Player _playerNode;

		/// <summary>
		/// Indica si el menú de pausa está actualmente activo
		/// </summary>
		private bool _isPaused;
		
		/// <summary>
		/// Inicialización del menú de pausa.
		/// Busca la referencia del jugador, oculta el menú y configura los botones.
		/// </summary>
		public override void _Ready()
		{
			
			_returnButton.Visible = true;
			_settingsButton.Visible = true;
			_exitButton.Visible = true;
			_optionsMenu.Visible = false;
			
			Visible = false;
		}
    
		/// <summary>
		/// Procesamiento del menú de pausa cada frame.
		/// Detecta cuando el usuario presiona la acción "pause" y alterna el estado de pausa.
		/// Se procesa incluso cuando el árbol de escenas está en pausa.
		/// </summary>
		/// <param name="delta">Tiempo transcurrido desde el último frame</param>
		public override void _Process(double delta)
		{
			if (Input.IsActionJustPressed("pause"))
			{
				if (_isPaused)
				{
					_resumeGame();
				}
				else
				{
					_tryPauseGame();
				}
			}
		}
    	
		/// <summary>
		/// Intenta pausar el juego solo si el jugador no ha realizado movimiento.
		/// Muestra un mensaje en la consola si se intenta pausar después de moverse.
		/// </summary>
		private void _tryPauseGame()
		{
			if (_playerNode != null && _playerNode.HasMoved())
			{
				GD.PrintErr("¡No puedes pausar el juego después de moverte!");
				return;
			}

			_pauseGame();
		}

		/// <summary>
		/// Pausa el juego deteniendo el input del jugador y mostrando el menú.
		/// No pausa el árbol de escenas completo para permitir que los botones funcionen.
		/// </summary>
		private void _pauseGame()
		{
			_isPaused = true;
			if (_playerNode != null)
			{
				_playerNode.SetPhysicsProcess(false);
			}
			Visible = true;
		}

		/// <summary>
		/// Reanuda el juego permitiendo input del jugador y ocultando el menú.
		/// </summary>
		private void _resumeGame()
		{
			_isPaused = false;
			if (_playerNode != null)
			{
				_playerNode.SetPhysicsProcess(true);
			}

			Visible = false;
			_optionsMenu.Visible = false;
			_returnButton.Visible = true;
			_settingsButton.Visible = true;
			_exitButton.Visible = true;
		}

		/// <summary>
		/// Alterna la visibilidad entre el menú principal y el menú de opciones.
		/// </summary>
		public void OnSettingsPressed()
		{
			ButtonTools.PlayPressAnimation(
				_settingsButton,
				() =>
				{
					Visibilities(); 
				});
		}

		/// <summary>
		/// Reanuda el juego desde el menú de pausa.
		/// </summary>
		public void OnReturnPressed()
		{
			ButtonTools.PlayPressAnimation(
				_returnButton,
				() =>
				{
					_resumeGame(); 
				});
		}

		/// <summary>
		/// Regresa al menú principal desde el menú de pausa.
		/// Reanuda el juego y carga la escena del menú principal.
		/// </summary>
		public void OnExitPressed()
		{
			ButtonTools.PlayPressAnimation(
				_exitButton,
				() =>
				{
					if (_playerNode != null)
					{
						_playerNode.SetPhysicsProcess(true);
					}
					GetTree().ChangeSceneToFile("res://scena/Menus/main_menu.tscn"); 
				});
		}

		/// <summary>
		/// Alterna la visibilidad de los botones del menú principal y del menú de opciones.
		/// </summary>
		internal void Visibilities()
		{
			_returnButton.Visible = !_returnButton.Visible;
			_settingsButton.Visible = !_settingsButton.Visible;
			_exitButton.Visible = !_exitButton.Visible;
			_optionsMenu.Visible = !_optionsMenu.Visible;
		}
	}
}