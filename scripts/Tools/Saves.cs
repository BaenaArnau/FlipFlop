using Godot;
using System;

namespace FlipFlop.scripts.Tools
{
    /// <summary>
    /// Record que almacena todos los datos de configuración del juego.
    /// Incluye opciones de video (resolución, antialiasing, VSync, FPS) y audio (volúmenes de distintos buses).
    /// </summary>
    /// <param name="Antialiasing">Configuración de anti-aliasing seleccionada</param>
    /// <param name="Vsync">Configuración de VSync (habilitado/deshabilitado)</param>
    /// <param name="Screen">Modo de pantalla (ventana, pantalla completa, sin bordes)</param>
    /// <param name="FpsLock">Estado del bloqueo de FPS (habilitado/deshabilitado)</param>
    /// <param name="FpsValue">Valor máximo de FPS cuando está bloqueado</param>
    /// <param name="Shadows">Calidad de sombras seleccionada</param>
    /// <param name="Master">Volumen del bus master (0.0 - 1.0)</param>
    /// <param name="Music">Volumen del bus de música (0.0 - 1.0)</param>
    /// <param name="SoundFx">Volumen del bus de efectos de sonido (0.0 - 1.0)</param>
    /// <param name="Enviroment">Volumen del bus ambiental (0.0 - 1.0)</param>
    /// <param name="UiSound">Volumen del bus de sonidos de interfaz (0.0 - 1.0)</param>
    public record OptionsData(
        int Antialiasing,
        int Vsync,
        int Screen,
        int FpsLock,
        int FpsValue,
        int Shadows,
        float Master,
        float Music,
        float SoundFx,
        float Enviroment,
        float UiSound);

    /// <summary>
    /// Gestiona la persistencia de configuración del juego usando archivos ConfigFile.
    /// Lee y escribe la configuración de video y audio en el archivo "user://settings.cfg".
    /// </summary>
    public class Saves
    {
        /// <summary>
        /// Archivo de configuración de Godot para almacenar datos
        /// </summary>
        readonly ConfigFile _config = new ConfigFile();

        /// <summary>
        /// Constructor del sistema de guardado.
        /// </summary>
        /// <param name="isSave">Si es true, prepara para guardar; si es false, carga la configuración existente</param>
        public Saves(bool isSave)
        {
            if (isSave)
            {
                try
                {

                }
                catch (Exception)
                {

                }
            }
            else
            {
                Load("user://settings.cfg");
            }
        }

        /// <summary>
        /// Carga la configuración desde un archivo ConfigFile.
        /// Si el archivo no existe, crea uno nuevo.
        /// </summary>
        /// <param name="filename">Ruta del archivo a cargar (ej: "user://settings.cfg")</param>
        public void Load(string filename)
        {
            try
            {
                _config.Load(filename);
            }
            catch (Exception)
            {
                _config.Save(filename);
            }
        }

        /// <summary>
        /// Guarda toda la configuración de opciones en el archivo de configuración.
        /// Almacena las opciones de video y audio en la ubicación "user://settings.cfg".
        /// </summary>
        /// <param name="antialiasing">Índice de configuración de anti-aliasing</param>
        /// <param name="vsync">Índice de configuración de VSync</param>
        /// <param name="screen">Índice de modo de pantalla</param>
        /// <param name="fpslock">Índice de estado de bloqueo de FPS</param>
        /// <param name="fpsValue">Valor máximo de FPS</param>
        /// <param name="shadows">Índice de calidad de sombras</param>
        /// <param name="master">Volumen del bus master</param>
        /// <param name="music">Volumen del bus de música</param>
        /// <param name="soundfx">Volumen del bus de efectos de sonido</param>
        /// <param name="enviroment">Volumen del bus ambiental</param>
        /// <param name="uisound">Volumen del bus de sonidos de interfaz</param>
        public void SaveOptions(
            int antialiasing,
            int vsync,
            int screen,
            int fpslock,
            int fpsValue,
            int shadows,
            double master,
            double music,
            double soundfx,
            double enviroment,
            double uisound)
        {
            _config.SetValue("Video", "antialiasing", antialiasing);
            _config.SetValue("Video", "vsync", vsync);
            _config.SetValue("Video", "screen", screen);
            _config.SetValue("Video", "fpslock", fpslock);
            _config.SetValue("Video", "fps_value", fpsValue);
            _config.SetValue("Video", "shadows", shadows);

            _config.SetValue("Sound", "master", master);
            _config.SetValue("Sound", "music", music);
            _config.SetValue("Sound", "soundfx", soundfx);
            _config.SetValue("Sound", "enviroment", enviroment);
            _config.SetValue("Sound", "uisound", uisound);

            _config.Save("user://settings.cfg");
        }

        /// <summary>
        /// Carga y retorna todos los datos de configuración guardados.
        /// Si algún valor no existe en el archivo, retorna un valor por defecto.
        /// </summary>
        /// <returns>Record OptionsData con todos los valores de configuración cargados</returns>
        public OptionsData LoadOptions()
        {
            return new OptionsData(
                (int)_config.GetValue("Video", "antialiasing", 0),
                (int)_config.GetValue("Video", "vsync", 0),
                (int)_config.GetValue("Video", "screen", 0),
                (int)_config.GetValue("Video", "fpslock", 0),
                (int)_config.GetValue("Video", "fps_value", 60),
                (int)_config.GetValue("Video", "shadows", 0),
                (float)(double)_config.GetValue("Sound", "master", 1.0),
                (float)(double)_config.GetValue("Sound", "music", 1.0),
                (float)(double)_config.GetValue("Sound", "soundfx", 1.0),
                (float)(double)_config.GetValue("Sound", "enviroment", 1.0),
                (float)(double)_config.GetValue("Sound", "uisound", 1.0)
            );
        }
    }
}
