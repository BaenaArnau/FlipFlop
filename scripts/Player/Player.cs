using FlipFlop.Scripts.Menus;
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
		private bool _deathHandled;

		/// <summary>
		/// Indica si el jugador ha realizado movimiento horizontal desde el inicio de la partida
		/// </summary>
		private bool _hasMoved;

		/// <summary>
		/// Referencia al área de gravedad que controla la inversión de gravedad
		/// </summary>
		[Export] private NodePath _areaGravityPath;
		private Gravity _areaGravity;
		
		[Export] private DeadMenu _deadMenu;
		
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
			{
				if (!_deathHandled)
				{
					_deadMenu?.ChangeVisibility(true);
					_deathHandled = true;
					QueueFree();
				}
				return;
			}

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
			if ((Input.IsActionJustPressed("flip") && IsOnFloor()) || 
			    (Input.IsActionJustPressed("flip") && IsOnCeiling()))
			{
				if (_areaGravity != null)
				{
					_areaGravity.ChangeGravity();
				}
				UpDirection = -UpDirection;
				_hasMoved = true;
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
			float direction = Input.GetAxis("move_left", "move_rigth");
			if (direction != 0)
			{
				velocity.X = direction * Speed;
				_hasMoved = true;
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

		/// <summary>
		/// Indica si el jugador se ha movido desde el inicio de la partida.
		/// </summary>
		/// <returns>Verdadero si el jugador ha presionado alguna tecla de movimiento, falso si no</returns>
		public bool HasMoved()
		{
			return _hasMoved;
		}
	}
}

