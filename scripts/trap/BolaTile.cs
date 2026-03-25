using Godot;
using System;
using FlipFlop.Scripts.Player;

namespace FlipFlop.Scripts.Trap
{
	public partial class BolaTile : RigidBody2D
    {
        
          public override void _Ready()
          {
          }

          public void OnAreaBodyEntered(Node body)
          {
              KillIfPlayer(body);
          }
          

          private static void KillIfPlayer(Node body)
          {
                if (body is Player.Player player)
                {
                  player.IsDead = true;
                }
        } 
    }
}

