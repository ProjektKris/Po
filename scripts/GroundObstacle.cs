using Godot;
using System;

public class GroundObstacle : StaticBody2D//RigidBody2D
{
    // exported variables
    [Export] public float speed = 400f; // in px/s
    [Export] public Vector2 spawn_position = new Vector2(1280, 450);
    [Export] public Vector2 end_position = new Vector2(-100, 450);

    public override void _Process(float delta)
    {
        if (Position.x >= end_position.x)
        {
            Position -= new Vector2(speed * delta, 0);
        } else {
			GD.Print("GroundObstacle leaving");
			QueueFree();
		}
    }
}
