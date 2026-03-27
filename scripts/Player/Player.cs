using FlipFlop.Scripts.Menus;
using Godot;
using FlipFlop.Scripts.Trap;

namespace FlipFlop.Scripts.Player
{
	public partial class Player : CharacterBody2D
	{
		public const float Speed = 400.0f;
		public const float JumpForce = -400.0f;

		public bool IsDead = false;
		private bool _deathHandled;
		private bool _hasMoved;
		private bool _gravityFlipped = false;

		[Export] private NodePath _areaGravityPath;
		private Gravity _areaGravity;
		
		[Export] private DeadMenu _deadMenu;

		private AnimatedSprite2D _animatedSprite;

		public override void _Ready()
		{
			if (_areaGravityPath != null)
			{
				_areaGravity = GetNode<Gravity>(_areaGravityPath);
			}

			_animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");

			if (_animatedSprite != null)
			{
				_animatedSprite.Play("idle");
			}
		}
		
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

			_updateAnimations();
		}

		private Vector2 _handleGravity(Vector2 velocity, float delta)
		{
			if (!IsOnFloor())
			{
				velocity = _addGravity(velocity, delta);
			}
			return velocity;
		}

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
				_gravityFlipped = !_gravityFlipped;
				_hasMoved = true;

				if (_animatedSprite != null)
				{
					_animatedSprite.FlipV = _gravityFlipped;
				}
			}
		}

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

		private void _updateAnimations()
		{
			if (_animatedSprite == null) return;

			float direction = Input.GetAxis("move_left", "move_rigth");

			if (direction != 0)
			{
				_animatedSprite.FlipH = direction > 0;

				if (_animatedSprite.Animation != "run")
				{
					_animatedSprite.Play("run");
				}
			}
			else
			{
				if (_animatedSprite.Animation != "idle")
				{
					_animatedSprite.Play("idle");
				}
			}
		}

		private Vector2 _addGravity(Vector2 velocity, float delta)
		{
			Vector2 gravity = GetGravity();
			return velocity + gravity * delta;
		}

		public bool HasMoved()
		{
			return _hasMoved;
		}
	}
}