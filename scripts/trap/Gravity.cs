using Godot;
using System;
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
		private HashSet<Area2D> _deadAreaConnections = new HashSet<Area2D>();
		
		/// <summary>
		/// Inicialización del área de gravedad.
		/// Conecta los eventos de entrada y salida de cuerpos del área.
		/// </summary>
		public override void _Ready()
		{
			BodyEntered += OnBodyEntered;
			BodyExited += OnBodyExited;
		}
		
		/// <summary>
		/// Invierte la dirección de la gravedad del área y desactiva/activa el monitoreo de las áreas de muerte.
		/// </summary>
		public void ChangeGravity()
		{
			GravityDirection = -GravityDirection;
			foreach (RigidBody2D rigidBody in _rigidBodies)
			{
				if (rigidBody.GetNode("DeadArea") is Area2D area)
					area.Monitoring = !area.Monitoring;
			}
		}
		
		/// <summary>
		/// Detecta cuando un cuerpo entra en el área de muerte y marca al jugador como muerto.
		/// </summary>
		/// <param name="body">Nodo que entró en el área de muerte</param>
		public void OnDeadAreaBodyEntered(Node body)
		{
			if (body is Player.Player player)
				player.IsDead = true;
		}
		
		/// <summary>
		/// Detecta cuando un RigidBody2D entra en el área de gravedad.
		/// Añade el objeto a la lista y conecta automáticamente su señal de muerte (DeadArea).
		/// </summary>
		/// <param name="body">RigidBody2D que entra en el área de gravedad</param>
		public void OnBodyEntered(Node body)
		{
			if (body is RigidBody2D rigidBody)
			{
				if (!_rigidBodies.Contains(rigidBody))
				{
					_rigidBodies.Add(rigidBody);
				}
				
				if (rigidBody.GetNode("DeadArea") is Area2D deadArea && !_deadAreaConnections.Contains(deadArea))
				{
					deadArea.BodyEntered += OnDeadAreaBodyEntered;
					_deadAreaConnections.Add(deadArea);
				}
			}
		}
		
		/// <summary>
		/// Detecta cuando un RigidBody2D sale del área de gravedad.
		/// Elimina el objeto de la lista y desconecta su señal de muerte.
		/// </summary>
		/// <param name="body">RigidBody2D que sale del área de gravedad</param>
		public void OnBodyExited(Node body)
		{
			if (body is RigidBody2D rigidBody)
			{
				_rigidBodies.Remove(rigidBody);
				
				if (rigidBody.GetNode("DeadArea") is Area2D deadArea && _deadAreaConnections.Contains(deadArea))
				{
					deadArea.BodyEntered -= OnDeadAreaBodyEntered;
					_deadAreaConnections.Remove(deadArea);
				}
			}
		}
	}
}