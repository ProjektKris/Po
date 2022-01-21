using Godot;
using System;

namespace Game
{
    public class GroundObstacle : StaticBody2D//RigidBody2D
    {
        // exported variables
        [Export] public float Speed = 400f; // in px/s
        [Export] public Vector2 EndPosition = new Vector2(-100, 450);

        public override void _Process(float delta)
        {
            if (Position.x >= EndPosition.x)
            {
                Position -= new Vector2(Speed * delta, 0);
            }
            else
            {
                GD.Print("GroundObstacle leaving");
                QueueFree();
            }
        }
    }
}
