using Godot;
using System;

public class Player : RigidBody2D
{
    // signals
    [Signal] delegate void hit();
    // exported variables
    [Export] public float jump_force = 600f;
    [Export] public float crouch_force = 10000f;
    [Export] public float ground_position = 425f;

    // private variables
    private Vector2 screen_size;

    // nodes
    private AnimatedSprite animated_sprite;
    private CollisionShape2D default_collision;
    private CollisionShape2D crouch_collision;

    public override void _Ready()
    {
        // assign variables
        animated_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        default_collision = GetNode<CollisionShape2D>("DefaultCollision");
        crouch_collision = GetNode<CollisionShape2D>("CrouchCollision");
        screen_size = GetViewport().Size;

        // signals
        Connect("body_entered", this, nameof(_on_Player_body_entered));
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_up"))
        {
            if (Position.y >= ground_position)
            {
                LinearVelocity += new Vector2(0, -jump_force);
                animated_sprite.Animation = "default";
            }
        }
    }
    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_down"))
        {
            AppliedForce += new Vector2(0, crouch_force);

            animated_sprite.Animation = "crouch";
            crouch_collision.Disabled = false;
            default_collision.Disabled = true;
        }
        else
        {
            AppliedForce = new Vector2(0, 0);

            animated_sprite.Animation = "default";
            crouch_collision.Disabled = true;
            default_collision.Disabled = false;
        }
    }
    public void _on_Player_body_entered(StaticBody2D body)
    {
        GD.Print("on player body entered");
        if (body.IsInGroup("obstacle"))
        {
            GD.Print("player collided with an obstacle");

            Mode = RigidBody2D.ModeEnum.Static;

            // hide
            Hide();

            // disable collision
            // default_collision.Disabled = true;
            // crouch_collision.Disabled = true;

            // inform main process that the player collided with an obstacle
            EmitSignal("hit");
        }
    }

    public void start(Vector2 pos)
    {
        // set position
        Position = pos;

        GD.Print("showing player");
        // unhide
        Show();

        // enable collision
        // default_collision.Disabled = false;
        // crouch_collision.Disabled = false;

        Mode = RigidBody2D.ModeEnum.Character;
    }
}
