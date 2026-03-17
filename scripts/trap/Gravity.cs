using Godot;
using System;
using System.Collections.Generic;

namespace FlipFlop.Scripts.Trap
{
	public partial class Gravity : Area2D
	{
		public void ChangeGravity()
		{
			GravityDirection = -GravityDirection;
		}
	}
}