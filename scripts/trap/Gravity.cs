using Godot;
using System.Collections.Generic;

namespace FlipFlop.Scripts.Trap
{
	/// <summary>
	/// Controla el área de gravedad personalizada y gestiona los objetos RigidBody2D dentro de ella.
	/// Permite invertir la dirección de la gravedad y conectar dinámicamente señales de muerte a los objetos.
	/// </summary>
	public partial class Gravity : Area2D
	{
		/// <summary>
		/// Lista de RigidBody2D actualmente dentro del área de gravedad
		/// </summary>
		private List<RigidBody2D> _rigidBodies = new List<RigidBody2D>();

		/// <summary>
		/// Invierte la dirección de la gravedad del área y desactiva/activa el monitoreo de las áreas de muerte.
		/// </summary>
		public void ChangeGravity()
		{
			GravityDirection = -GravityDirection;
			foreach (RigidBody2D rigidBody in _rigidBodies)
			{
				rigidBody.LinearVelocity += GravityDirection;
			}
		}

		public void OnBodyEntered(Node body)
		{
			if (body is RigidBody2D rigidBody)
			{
				_rigidBodies.Add(rigidBody);
			}
		}
	}
}