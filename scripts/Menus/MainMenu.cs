using Godot;
using FlipFlop.scripts.Tools;

namespace FlipFlop.Scripts.Menus
{
	/// <summary>
	/// Menú principal del juego.
	/// Gestiona la navegación entre el menú principal, opciones y el inicio del juego.
	/// </summary>
	public partial class MainMenu : Control
    {
    	/// <summary>
    	/// Referencia al menú de opciones
    	/// </summary>
    	[Export] private Control _optionsMenu;

    	/// <summary>
    	/// Botón para iniciar el juego
    	/// </summary>
	    [Export] private Button _playButton;

	    /// <summary>
	    /// Botón para salir del juego
	    /// </summary>
	    [Export] private Button _exitButton;

	    /// <summary>
	    /// Botón para abrir el menú de configuración
	    /// </summary>
	    [Export] private Button _settingsButton;
    	
    	/// <summary>
    	/// Inicialización del menú principal.
    	/// Muestra los botones principales y oculta el menú de opciones.
    	/// </summary>
    	public override void _Ready()
    	{
		    _playButton.Visible = true;
		    _settingsButton.Visible = true;
		    _exitButton.Visible = true;
		    _optionsMenu.Visible = false;
    	}
    
    	/// <summary>
    	/// Procesamiento del menú principal (no utilizado).
    	/// </summary>
    	/// <param name="delta">Tiempo transcurrido desde el último frame</param>
    	public override void _Process(double delta)
    	{
    	}
    	
    	/// <summary>
    	/// Alterna la visibilidad entre el menú principal y el menú de opciones.
    	/// </summary>
	    public void OnSettingsPressed()
	    {
		    ButtonTools.PlayPressAnimation(
			    _settingsButton,
			    () => { Visibilities(); });
	    }

	    /// <summary>
	    /// Inicia el juego cargando la escena principal del mapa.
	    /// </summary>
	    public void OnPlayPressed()
	    {
		    ButtonTools.PlayPressAnimation(
			    _playButton,
			    () => { GetTree().ChangeSceneToFile("res://scena/Maps/main.tscn"); });
	    }

	    /// <summary>
	    /// Cierra la aplicación del juego.
	    /// </summary>
	    public void OnExitPressed()
	    {
		    ButtonTools.PlayPressAnimation(
			    _exitButton,
			    () => { GetTree().Quit(); });
	    }

	    /// <summary>
	    /// Alterna la visibilidad de los botones del menú principal y del menú de opciones.
	    /// </summary>
	    internal void Visibilities()
	    {
		    _playButton.Visible = !_playButton.Visible;
		    _settingsButton.Visible = !_settingsButton.Visible;
		    _exitButton.Visible = !_exitButton.Visible;
		    _optionsMenu.Visible = !_optionsMenu.Visible;
	    }
    }
}


