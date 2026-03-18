using Godot;
using FlipFlop.Scripts.Trap;

namespace FlipFlop.Scripts.Player
{
	/// <summary>
	/// Controlador del personaje jugador.
	/// Gestiona el movimiento, saltos, gravedad invertible y detección de muerte.
	/// </summary>
	public partial class Player : CharacterBody2D
	{
		/// <summary>
		/// Velocidad de movimiento horizontal del jugador (px/s)
		/// </summary>
		public const float Speed = 300.0f;

		/// <summary>
		/// Fuerza de salto inicial del jugador (px/s)
		/// </summary>
		public const float JumpForce = -400.0f;

		/// <summary>
		/// Indica si el jugador está muerto
		/// </summary>
		public bool IsDead = false;

		/// <summary>
		/// Referencia al área de gravedad que controla la inversión de gravedad
		/// </summary>
		[Export] private NodePath _areaGravityPath;
		private Gravity _areaGravity;
		
		/// <summary>
		/// Inicialización del jugador al entrar en la escena.
		/// Resuelve la referencia del área de gravedad desde el NodePath.
		/// </summary>
		public override void _Ready()
		{
			if (_areaGravityPath != null)
			{
				_areaGravity = GetNode<Gravity>(_areaGravityPath);
			}
		}
		
		/// <summary>
		/// Procesamiento físico del jugador cada frame.
		/// Gestiona la gravedad, entrada de salto, movimiento y colisiones.
		/// </summary>
		/// <param name="delta">Tiempo transcurrido desde el último frame (en segundos)</param>
		public override void _PhysicsProcess(double delta)
		{
			if (IsDead)
				QueueFree();
			
			Vector2 velocity = Velocity;

			velocity = _handleGravity(velocity, (float)delta);
			_handleJump();
			velocity = _handleMovement(velocity);

			Velocity = velocity;
			MoveAndSlide();
		}

		/// <summary>
		/// Aplica la gravedad a la velocidad cuando el jugador está en el aire.
		/// </summary>
		/// <param name="velocity">Velocidad actual del jugador</param>
		/// <param name="delta">Tiempo transcurrido desde el último frame</param>
		/// <returns>Velocidad modificada con la gravedad aplicada</returns>
		private Vector2 _handleGravity(Vector2 velocity, float delta)
		{
			if (!IsOnFloor())
			{
				velocity = _addGravity(velocity, delta);
			}
			return velocity;
		}

		/// <summary>
		/// Gestiona el input de salto y la inversión de gravedad.
		/// Se dispara cuando el jugador presiona la acción "ui_accept" desde el suelo o techo.
		/// </summary>
		private void _handleJump()
		{
			if ((Input.IsActionJustPressed("ui_accept") && IsOnFloor()) || 
			    (Input.IsActionJustPressed("ui_accept") && IsOnCeiling()))
			{
				if (_areaGravity != null)
				{
					_areaGravity.ChangeGravity();
				}
				UpDirection = -UpDirection;
			}
		}

		/// <summary>
		/// Gestiona el movimiento horizontal del jugador según la entrada del usuario.
		/// Incluye aceleración y desaceleración suave.
		/// </summary>
		/// <param name="velocity">Velocidad actual del jugador</param>
		/// <returns>Velocidad modificada con el movimiento aplicado</returns>
		private Vector2 _handleMovement(Vector2 velocity)
		{
			Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}
			return velocity;
		}

		/// <summary>
		/// Calcula y aplica la gravedad a la velocidad del jugador.
		/// </summary>
		/// <param name="velocity">Velocidad actual del jugador</param>
		/// <param name="delta">Tiempo transcurrido desde el último frame</param>
		/// <returns>Velocidad con la gravedad aplicada</returns>
		private Vector2 _addGravity(Vector2 velocity, float delta)
		{
			Vector2 gravity = GetGravity();
			return velocity + gravity * delta;
		}
	}
}

