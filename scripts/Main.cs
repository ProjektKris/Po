using Godot;
using System;

public class Main : Node2D
{
    // signals
    [Signal] delegate void game_ended();

    // exported variables
    [Export] PackedScene groundObstacleScene;
    [Export] public float starting_speed = 400f;
    [Export] public float speed_acceleration = 20f; // +xspeed/second

    // variables
    private int score;

    // nodes
    private RigidBody2D player;
    private CanvasLayer UI;
    private Timer scoreTimer;
    private Timer obstacleTimer;
    private Timer startTimer;
    private StaticBody2D[] loaded_obstacles;

    private Position2D playerPosition;
    private Position2D obstacleStartPos;


    public override void _Ready()
    {
        // assign variables
        groundObstacleScene = (PackedScene)ResourceLoader.Load("res://scenes/GroundObstacle.tscn");

        player = GetNode<RigidBody2D>("Player");
        UI = GetNode<CanvasLayer>("UI");
        scoreTimer = GetNode<Timer>("ScoreTimer");
        obstacleTimer = GetNode<Timer>("ObstacleTimer");
        startTimer = GetNode<Timer>("StartTimer");
        playerPosition = GetNode<Position2D>("PlayerPosition");
        obstacleStartPos = GetNode<Position2D>("GroundObstacleStart");

        // signals
        UI.Connect("startGame", this, nameof(newGame));
        obstacleTimer.Connect("timeout", this, nameof(onObstacleTimerTimeout));
        scoreTimer.Connect("timeout", this, nameof(onScoreTimerTimeout));
        startTimer.Connect("timeout", this, nameof(onStartTimerTimeout));
        player.Connect("hit", this, nameof(gameOver));
    }
    // public override void _Process(float delta)
    // {

    // }
    public void gameOver()
    {
        // stop score timer
        scoreTimer.Stop();

        // stop obstacle timer
        obstacleTimer.Stop();

        // show hud game over
        UI.Call("showGameOver");

        // remove old enemies
        GetTree().CallGroup("obstacle", "queue_free");

        // stop music
    }
    void newGame()
    {
        // reset score
        score = 0;

        // set player position
        player.Call("start", playerPosition.Position);

        // start countdown timer
        startTimer.Start();

        // update hud score
        UI.Call("updateScore", score);

        // show message get ready
        UI.Call("showMessage", "Get Ready");

        // play music
    }
    public void onObstacleTimerTimeout() {
        GD.Print("spawning ground obstacle");

        StaticBody2D newObstacle = (StaticBody2D)groundObstacleScene.Instance();
        AddChild(newObstacle);
        newObstacle.Position = obstacleStartPos.Position;
    }
    public void onScoreTimerTimeout()
    {
        score += 1;

        UI.Call("updateScore", score);
    }

    public void onStartTimerTimeout() {
        GD.Print("start timer timeout");
        scoreTimer.Start();
        obstacleTimer.Start();
    }

}
