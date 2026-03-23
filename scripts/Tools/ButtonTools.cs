using System;
using Godot;

namespace FlipFlop.scripts.Tools
{
    /// <summary>
    /// Clase utilidad que proporciona funciones para animar botones.
    /// Contiene métodos para reproducir animaciones de interacción de usuario.
    /// </summary>
    public static class ButtonTools
    {
        /// <summary>
        /// Escala del botón cuando está presionado
        /// </summary>
        private static readonly Vector2 PressedScale = new(0.85f, 0.85f);

        /// <summary>
        /// Escala máxima del botón durante la animación de rebote
        /// </summary>
        private static readonly Vector2 BounceScale = new(1.1f, 1.1f);

        /// <summary>
        /// Escala normal del botón
        /// </summary>
        private static readonly Vector2 NormalScale = Vector2.One;

        /// <summary>
        /// Duración de la animación de presión (encogimiento)
        /// </summary>
        private const float PressDuration = 0.1f;

        /// <summary>
        /// Duración de la animación de rebote (expansión)
        /// </summary>
        private const float BounceDuration = 0.12f;

        /// <summary>
        /// Duración de la animación de estabilización (volver a normal)
        /// </summary>
        private const float SettleDuration = 0.08f;

        /// <summary>
        /// Reproduce una animación de "press" en el botón: se encoge, rebota y vuelve a su tamaño original.
        /// La animación tiene tres fases:
        /// 1. Encogimiento (10ms)
        /// 2. Rebote hacia arriba (12ms)
        /// 3. Estabilización (8ms)
        /// Ejecuta el callback <paramref name="onFinished"/> al terminar la animación.
        /// </summary>
        /// <param name="button">Control (botón) al que aplicar la animación</param>
        /// <param name="onFinished">Acción opcional a ejecutar cuando termine la animación</param>
        public static void PlayPressAnimation(Control button, Action onFinished = null)
        {
            // Asegurarse de que el pivot esté en el centro del botón
            button.PivotOffset = button.Size / 2;

            var tween = button.CreateTween();

            // Paso 1: Encoger (irse para dentro)
            tween.TweenProperty(button, "scale", PressedScale, PressDuration)
                .SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Cubic);

            // Paso 2: Rebotar haciéndose más grande
            tween.TweenProperty(button, "scale", BounceScale, BounceDuration)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Back);

            // Paso 3: Volver al tamaño normal
            tween.TweenProperty(button, "scale", NormalScale, SettleDuration)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Cubic);

            // Ejecutar el callback cuando termine la animación
            if (onFinished != null)
            {
                tween.TweenCallback(Callable.From(onFinished));
            }
        }
    }
}
