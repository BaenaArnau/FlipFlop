using FlipFlop.scripts.Tools;
using Godot;

namespace FlipFlop.Scripts.Menus
{
    /// <summary>
    /// Menú de opciones que gestiona configuración de video, audio y controles del juego.
    /// Permite ajustar resolución, anti-aliasing, VSync, FPS, sombras y volúmenes de audio.
    /// </summary>
    public partial class OptionsMenu : Control
    {
        [ExportGroup("References")]
        [Export] private MenuPausa _puaseMenu;
        
        /// <summary>
        /// Referencia al menú principal para volver a él
        /// </summary>
        [Export] private MainMenu _mainMenu;
        
        /// <summary>
        /// Botón selector de anti-aliasing (Off, FXAA, TAA, MSAA 2x/4x/8x)
        /// </summary>
        [ExportGroup("Video")]
        [Export]
        private OptionButton _antialiasing;

        /// <summary>
        /// Botón selector de VSync (Off/On)
        /// </summary>
        [Export]
        private OptionButton _vsync;

        /// <summary>
        /// Botón selector de modo de pantalla (Windowed, Fullscreen, Borderless)
        /// </summary>
        [Export]
        private OptionButton _screen;

        /// <summary>
        /// Botón selector de bloqueo de FPS (Off/On)
        /// </summary>
        [Export]
        private OptionButton _fpslock;

        /// <summary>
        /// Slider para ajustar el valor máximo de FPS
        /// </summary>
        [Export]
        private HSlider _fpsslider;

        /// <summary>
        /// SpinBox para ajustar el valor máximo de FPS (vinculado al slider)
        /// </summary>
        [Export]
        private SpinBox _fpsspinbox;

        /// <summary>
        /// Botón selector de calidad de sombras (Hard, Soft Very Low, Soft Low, Soft Medium, Soft High, Soft Ultra)
        /// </summary>
        [Export]
        private OptionButton _shadows;

        /// <summary>
        /// Slider del volumen master
        /// </summary>
        [ExportGroup("Sound")]
        [Export]
        private HSlider _master;

        /// <summary>
        /// SpinBox del volumen master (vinculado al slider)
        /// </summary>
        [Export]
        private SpinBox _masterSpinBox;

        /// <summary>
        /// Slider del volumen de música
        /// </summary>
        [Export]
        private HSlider _music;

        /// <summary>
        /// SpinBox del volumen de música (vinculado al slider)
        /// </summary>
        [Export]
        private SpinBox _musicSpinBox;

        /// <summary>
        /// Slider del volumen de efectos de sonido
        /// </summary>
        [Export]
        private HSlider _soundfx;

        /// <summary>
        /// SpinBox del volumen de efectos de sonido (vinculado al slider)
        /// </summary>
        [Export]
        private SpinBox _soundfxSpinBox;

        /// <summary>
        /// Slider del volumen ambiental
        /// </summary>
        [Export]
        private HSlider _enviroment;

        /// <summary>
        /// SpinBox del volumen ambiental (vinculado al slider)
        /// </summary>
        [Export]
        private SpinBox _enviromentSpinBox;

        /// <summary>
        /// Slider del volumen de sonidos de interfaz
        /// </summary>
        [Export]
        private HSlider _uisound;

        /// <summary>
        /// SpinBox del volumen de sonidos de interfaz (vinculado al slider)
        /// </summary>
        [Export]
        private SpinBox _uisoundSpinBox;

        /// <summary>
        /// Botón para guardar la configuración actual
        /// </summary>
        [ExportGroup("Control buttons")]
        [Export]
        private Button _saveButton;

        /// <summary>
        /// Botón para cerrar el menú de opciones
        /// </summary>
        [Export]
        private Button _exitButton;

        /// <summary>
        /// Sistema de guardado para persistir las opciones
        /// </summary>
        private Saves _saveSettings;

        /// <summary>
        /// Indica si el bloqueo de FPS está activado
        /// </summary>
        private bool _isFpslock;

        /// <summary>
        /// Valor actual del FPS máximo cuando está bloqueado
        /// </summary>
        private int _fpsValue = 60;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _saveSettings = new Saves(false);
            var options = _saveSettings.LoadOptions();

            // Video
            OnOptionButtonItemSelected(options.Antialiasing);
            OnVSyncButtonItemSelected(options.Vsync);
            OnWindowButtonItemSelected(options.Screen);
            _fpsValue = options.FpsValue;
            OnOptionFPSButtonItemSelected(options.FpsLock);
            OnOptionShadowButtonItemSelected(options.Shadows);

            // Sound
            OnHMasterSliderValueChanged(options.Master);
            OnHMusicSliderValueChanged(options.Music);
            OnHSoundFXSliderValueChanged(options.SoundFx);
            OnHEnviromentSliderValueChanged(options.Enviroment);
            OnHUISoundSliderValueChanged(options.UiSound);
        }

        /// <summary>
        /// Cambia la configuración de anti-aliasing según la selección.
        /// </summary>
        /// <param name="index">Índice de la opción seleccionada (0-5)</param>
        public void OnOptionButtonItemSelected(int index)
        {
            _antialiasing.Selected = index;
            switch (index)
            {
                case 0:
                    GD.Print("Antialiasing: Off");
                    GetViewport().Msaa2D = Viewport.Msaa.Disabled;
                    GetViewport().Msaa3D = Viewport.Msaa.Disabled;
                    GetViewport().UseTaa = false;
                    GetViewport().ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
                    break;
                case 1:
                    GD.Print("Antialiasing: FXAA");
                    GetViewport().Msaa2D = Viewport.Msaa.Disabled;
                    GetViewport().Msaa3D = Viewport.Msaa.Disabled;
                    GetViewport().UseTaa = false;
                    GetViewport().ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Fxaa;
                    break;
                case 2:
                    GD.Print("Antialiasing: TAA");
                    GetViewport().Msaa2D = Viewport.Msaa.Disabled;
                    GetViewport().Msaa3D = Viewport.Msaa.Disabled;
                    GetViewport().UseTaa = true;
                    GetViewport().ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
                    break;
                case 3:
                    GD.Print("Antialiasing: MSAA 2x");
                    GetViewport().Msaa2D = Viewport.Msaa.Msaa2X;
                    GetViewport().Msaa3D = Viewport.Msaa.Msaa2X;
                    GetViewport().UseTaa = false;
                    GetViewport().ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
                    break;
                case 4:
                    GD.Print("Antialiasing: MSAA 4x");
                    GetViewport().Msaa2D = Viewport.Msaa.Msaa4X;
                    GetViewport().Msaa3D = Viewport.Msaa.Msaa4X;
                    GetViewport().UseTaa = false;
                    GetViewport().ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
                    break;
                case 5:
                    GD.Print("Antialiasing: MSAA 8x");
                    GetViewport().Msaa2D = Viewport.Msaa.Msaa8X;
                    GetViewport().Msaa3D = Viewport.Msaa.Msaa8X;
                    GetViewport().UseTaa = false;
                    GetViewport().ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
                    break;
            }
        }

        /// <summary>
        /// Cambia la configuración de VSync según la selección.
        /// </summary>
        /// <param name="index">Índice de la opción (0=Off, 1=On)</param>
        public void OnVSyncButtonItemSelected(int index)
        {
            _vsync.Selected = index;
            if (index == 0)
            {
                GD.Print("VSync: Off");
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
            }
            else if (index == 1)
            {
                GD.Print("VSync: On");
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
            }
        }

        /// <summary>
        /// Cambia el modo de pantalla (Ventana, Pantalla completa, Sin bordes).
        /// </summary>
        /// <param name="index">Índice de la opción (0=Windowed, 1=Fullscreen, 2=Borderless)</param>
        public void OnWindowButtonItemSelected(int index)
        {
            _screen.Selected = index;
            switch (index)
            {
                case 0:
                    GD.Print("Screen Mode: Windowed");
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                    break;
                case 1:
                    GD.Print("Screen Mode: Fullscreen");
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                    break;
                case 2:
                    GD.Print("Screen Mode: Borderless");
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
                    break;
            }
        }

        /// <summary>
        /// Activa o desactiva el bloqueo de FPS y configura el valor máximo.
        /// </summary>
        /// <param name="index">Índice de la opción (0=Off, 1=On)</param>
        public void OnOptionFPSButtonItemSelected(int index)
        {
            _fpslock.Selected = index;
            if (index == 0)
            {
                GD.Print("FPS Lock: Off");
                _isFpslock = false;
                Engine.MaxFps = 0;
                _fpsslider.Editable = false;
                _fpsspinbox.Editable = false;
            }
            else if (index == 1)
            {
                GD.Print("FPS Lock: On");
                _isFpslock = true;
                Engine.MaxFps = _fpsValue;
                _fpsslider.Value = _fpsValue;
                _fpsspinbox.Value = _fpsValue;
                _fpsslider.Editable = true;
                _fpsspinbox.Editable = true;
            }
        }

        /// <summary>
        /// Cambia el FPS máximo cuando el slider cambia su valor (solo si el bloqueo está activado).
        /// </summary>
        /// <param name="value">Nuevo valor del FPS máximo</param>
        public void OnHSliderFPSValueChanged(float value)
        {
            if (_isFpslock)
            {
                _fpsValue = (int)value;
                _fpsspinbox.Value = _fpsValue;
                _fpsslider.Value = _fpsValue;
                Engine.MaxFps = _fpsValue;
            }
        }

        /// <summary>
        /// Cambia el FPS máximo cuando el spinbox cambia su valor (solo si el bloqueo está activado).
        /// </summary>
        /// <param name="value">Nuevo valor del FPS máximo</param>
        public void OnSpinFPSBoxValueChanged(float value)
        {
            if (_isFpslock)
            {
                _fpsValue = (int)value;
                _fpsspinbox.Value = _fpsValue;
                _fpsslider.Value = _fpsValue;
                Engine.MaxFps = _fpsValue;
            }
        }

        /// <summary>
        /// Cambia la calidad de las sombras del juego.
        /// </summary>
        /// <param name="index">Índice de la calidad (0=Hard, 1=Soft Very Low, 2=Soft Low, 3=Soft Medium, 4=Soft High, 5=Soft Ultra)</param>
        public void OnOptionShadowButtonItemSelected(int index)
        {
            _shadows.Selected = index;
            switch (index)
            {
                case 0:
                    GD.Print("Shadow Quality: Hard");
                    ApplyShadowQuality(RenderingServer.ShadowQuality.Hard);
                    break;
                case 1:
                    GD.Print("Shadow Quality: Soft Very Low");
                    ApplyShadowQuality(RenderingServer.ShadowQuality.SoftVeryLow);
                    break;
                case 2:
                    GD.Print("Shadow Quality: Soft Low");
                    ApplyShadowQuality(RenderingServer.ShadowQuality.SoftLow);
                    break;
                case 3:
                    GD.Print("Shadow Quality: Soft Medium");
                    ApplyShadowQuality(RenderingServer.ShadowQuality.SoftMedium);
                    break;
                case 4:
                    GD.Print("Shadow Quality: Soft High");
                    ApplyShadowQuality(RenderingServer.ShadowQuality.SoftHigh);
                    break;
                case 5:
                    GD.Print("Shadow Quality: Soft Ultra");
                    ApplyShadowQuality(RenderingServer.ShadowQuality.SoftUltra);
                    break;
            }
        }

        /// <summary>
        /// Aplica la calidad de sombras seleccionada a los servidores de renderizado.
        /// </summary>
        /// <param name="quality">Nivel de calidad de sombra a aplicar</param>
        private static void ApplyShadowQuality(RenderingServer.ShadowQuality quality)
        {
            // En entornos sin backend gráfico, RenderingServer puede no estar disponible.
            if (DisplayServer.GetName() == "headless")
            {
                return;
            }

            try
            {
                RenderingServer.DirectionalSoftShadowFilterSetQuality(quality);
                RenderingServer.PositionalSoftShadowFilterSetQuality(quality);
            }
            catch (System.Exception ex)
            {
                GD.PushWarning($"No se pudo aplicar la calidad de sombras: {ex.Message}");
            }
        }

        /// <summary>
        /// Cambia el volumen del bus master cuando el slider cambia.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen (0.0 - 1.0)</param>
        public void OnHMasterSliderValueChanged(float value)
        {
            _SetBusVolumeDbSafe(0, value);
            _masterSpinBox.Value = value;
            _master.Value = value;
        }

        /// <summary>
        /// Cambia el volumen del bus de música cuando el slider cambia.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen (0.0 - 1.0)</param>
        public void OnHMusicSliderValueChanged(float value)
        {
            _SetBusVolumeDbSafe(1, value);
            _musicSpinBox.Value = value;
            _music.Value = value;
        }

        /// <summary>
        /// Cambia el volumen del bus de efectos de sonido cuando el slider cambia.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen (0.0 - 1.0)</param>
        public void OnHSoundFXSliderValueChanged(float value)
        {
            _SetBusVolumeDbSafe(2, value);
            _soundfxSpinBox.Value = value;
            _soundfx.Value = value;
        }

        /// <summary>
        /// Cambia el volumen del bus ambiental cuando el slider cambia.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen (0.0 - 1.0)</param>
        public void OnHEnviromentSliderValueChanged(float value)
        {
            _SetBusVolumeDbSafe(3, value);
            _enviromentSpinBox.Value = value;
            _enviroment.Value = value;
        }

        /// <summary>
        /// Cambia el volumen del bus de sonidos de interfaz cuando el slider cambia.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen (0.0 - 1.0)</param>
        public void OnHUISoundSliderValueChanged(float value)
        {
            _SetBusVolumeDbSafe(4, value);
            _uisoundSpinBox.Value = value;
            _uisound.Value = value;
        }

        /// <summary>
        /// Cambia el volumen del bus master cuando el spinbox cambia.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen (0.0 - 1.0)</param>
        public void OnSpinBoxMasterValueChanged(float value)
        {
            _SetBusVolumeDbSafe(0, value);
            _masterSpinBox.Value = value;
            _master.Value = value;
        }

        /// <summary>
        /// Cambia el volumen del bus de música cuando el spinbox cambia.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen (0.0 - 1.0)</param>
        public void OnSpinBoxMusicValueChanged(float value)
        {
            _SetBusVolumeDbSafe(1, value);
            _music.Value = value;
            _musicSpinBox.Value = value;
        }

        /// <summary>
        /// Cambia el volumen del bus de efectos de sonido cuando el spinbox cambia.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen (0.0 - 1.0)</param>
        public void OnSpinBoxSoundFXValueChanged(float value)
        {
            _SetBusVolumeDbSafe(2, value);
            _soundfx.Value = value;
            _soundfxSpinBox.Value = value;
        }

        /// <summary>
        /// Cambia el volumen del bus ambiental cuando el spinbox cambia.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen (0.0 - 1.0)</param>
        public void OnSpinBoxEnviromentValueChanged(float value)
        {
            _SetBusVolumeDbSafe(3, value);
            _enviroment.Value = value;
            _enviromentSpinBox.Value = value;
        }

        /// <summary>
        /// Cambia el volumen del bus de sonidos de interfaz cuando el spinbox cambia.
        /// </summary>
        /// <param name="value">Nuevo valor del volumen (0.0 - 1.0)</param>
        public void OnSpinBoxUISoundValueChanged(float value)
        {
            _SetBusVolumeDbSafe(4, value);
            _uisound.Value = value;
            _uisoundSpinBox.Value = value;
        }

        /// <summary>
        /// Guarda la configuración actual del menú de opciones.
        /// </summary>
        public void OnSavePressed()
        {
            ButtonTools.PlayPressAnimation(
                _saveButton,
                () =>
                {
                    _saveSettings.SaveOptions(
                        _antialiasing.Selected,
                        _vsync.Selected,
                        _screen.Selected,
                        _fpslock.Selected,
                        _fpsValue,
                        _shadows.Selected,
                        _master.Value,
                        _music.Value,
                        _soundfx.Value,
                        _enviroment.Value,
                        _uisound.Value
                    );
                }
            );
        }

        /// <summary>
        /// Cierra el menú de opciones y retorna al menú principal.
        /// </summary>
        public void OnExitPressed()
        {
            ButtonTools.PlayPressAnimation(
                _exitButton,
                () =>
                {
                    if (_mainMenu != null)
                        _mainMenu.Visibilities();
                    else
                        _puaseMenu.Visibilities();
                }
            );
        }

        /// <summary>
        /// Establece el volumen de un bus de audio de forma segura, validando que exista.
        /// </summary>
        /// <param name="busIndex">Índice del bus de audio</param>
        /// <param name="value">Valor del volumen (0.0 - 1.0)</param>
        private void _SetBusVolumeDbSafe(int busIndex, float value)
        {
            if (busIndex >= 0 && busIndex < AudioServer.BusCount)
            {
                AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
            }
            else
            {
                GD.PushWarning($"Bus de audio {busIndex} no existe. Buses disponibles: {AudioServer.BusCount}");
            }
        }
    }
}
