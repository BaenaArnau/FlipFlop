using Godot;
using FlipFlop.Scripts.Trap;

namespace FlipFlop.Scripts.Player
{
	public partial class Player : CharacterBody2D
	{
		public const float Speed = 300.0f;
		public const float JumpForce = -400.0f;
		[Export] private Gravity _areaGravity;
		
		
		public override void _PhysicsProcess(double delta)
		{
			Vector2 velocity = Velocity;

			// Add the gravity.
			if (!IsOnFloor())
			{
				_addGravity(velocity, (float)delta);
			}

			// Handle Jump.
			if ((Input.IsActionJustPressed("ui_accept") && IsOnFloor()) || (Input.IsActionJustPressed("ui_accept") && IsOnCeiling()))
			{
				_areaGravity.ChangeGravity();
				UpDirection = -UpDirection;
			}

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}

			Velocity = velocity;
			MoveAndSlide();
		}

		private void _addGravity(Vector2 velocity, float delta)
		{
			Vector2 gravity = GetGravity();
			velocity += gravity * delta;
		}
	}
}