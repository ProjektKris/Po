using Godot;
using System;

public class Player : RigidBody2D
{
    // signals
    [Signal] delegate void Hit();
    // exported variables
    [Export] public float JumpForce = 600f;
    [Export] public float CrouchForce = 10000f;
    [Export] public float GroundPosition = 425f;

    // private variables
    private Vector2 _screenSize;

    // nodes
    private AnimatedSprite _animatedSprite;
    private CollisionShape2D _defaultCollision;
    private CollisionShape2D _crouchCollision;

    public override void _Ready()
    {
        // assign variables
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _defaultCollision = GetNode<CollisionShape2D>("DefaultCollision");
        _crouchCollision = GetNode<CollisionShape2D>("CrouchCollision");
        _screenSize = GetViewport().Size;

        // signals
        Connect("body_entered", this, nameof(BodyEnteredHandler));
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_up"))
        {
            if (Position.y >= GroundPosition)
            {
                LinearVelocity += new Vector2(0, -JumpForce);
                _animatedSprite.Animation = "default";
            }
        }
    }
    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_down"))
        {
            AppliedForce += new Vector2(0, CrouchForce);

            _animatedSprite.Animation = "crouch";
            _crouchCollision.Disabled = false;
            _defaultCollision.Disabled = true;
        }
        else
        {
            AppliedForce = new Vector2(0, 0);

            _animatedSprite.Animation = "default";
            _crouchCollision.Disabled = true;
            _defaultCollision.Disabled = false;
        }
    }
    public void BodyEnteredHandler(StaticBody2D body)
    {
        GD.Print("on player body entered");
        if (body.IsInGroup("obstacle"))
        {
            GD.Print("player collided with an obstacle");

            Mode = RigidBody2D.ModeEnum.Static;

            // hide
            Hide();

            // disable collision
            // _defaultCollision.Disabled = true;
            // _crouchCollision.Disabled = true;

            // inform main process that the player collided with an obstacle
            EmitSignal("Hit");
        }
    }

    public void Start(Vector2 pos)
    {
        // set position
        Position = pos;

        GD.Print("showing player");

        // unhide
        Show();

        // enable collision
        // _defaultCollision.Disabled = false;
        // _crouchCollision.Disabled = false;

        Mode = RigidBody2D.ModeEnum.Character;
    }
}
