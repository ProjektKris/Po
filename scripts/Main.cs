using Godot;
using System;

public class Main : Node2D
{
    [Export] PackedScene groundObstacleScene;

    private int score;

    private RigidBody2D _player;
    private CanvasLayer _ui;
    private Timer _scoreTimer;
    private Timer _obstacleTimer;
    private Timer _startTimer;

    private Position2D _playerPosition;
    private Position2D _obstacleStartPos;


    public override void _Ready()
    {
        _player = GetNode<RigidBody2D>("Player");
        _ui = GetNode<CanvasLayer>("UI");
        _scoreTimer = GetNode<Timer>("ScoreTimer");
        _obstacleTimer = GetNode<Timer>("ObstacleTimer");
        _startTimer = GetNode<Timer>("StartTimer");
        _playerPosition = GetNode<Position2D>("PlayerPosition");
        _obstacleStartPos = GetNode<Position2D>("GroundObstacleStart");

        // signals
        _ui.Connect("StartGame", this, nameof(NewGame));
        _obstacleTimer.Connect("timeout", this, nameof(ObstacleTimerTimeoutHandler));
        _scoreTimer.Connect("timeout", this, nameof(ScoreTimerTimeoutHandler));
        _startTimer.Connect("timeout", this, nameof(StartTimerTimeoutHandler));
        _player.Connect("Hit", this, nameof(GameOver));
    }
    // public override void _Process(float delta)
    // {

    // }
    public void GameOver()
    {
        // stop score timer
        _scoreTimer.Stop();

        // stop obstacle timer
        _obstacleTimer.Stop();

        // show hud game over
        _ui.Call("ShowGameOver");

        // remove old enemies
        GetTree().CallGroup("obstacle", "queue_free");

        // stop music
    }
    public void NewGame()
    {
        // reset score
        score = 0;

        // set player position
        _player.Call("Start", _playerPosition.Position);

        // start countdown timer
        _startTimer.Start();

        // update hud score
        _ui.Call("UpdateScore", score);

        // show message get ready
        _ui.Call("ShowMessage", "Get Ready");

        // play music
    }
    public void ObstacleTimerTimeoutHandler()
    {
        GD.Print("spawning ground obstacle");

        StaticBody2D newObstacle = (StaticBody2D)groundObstacleScene.Instance();
        AddChild(newObstacle);
        newObstacle.Position = _obstacleStartPos.Position;

        // rng for next timeout
        _obstacleTimer.WaitTime = (float)GD.RandRange(1.5f, 3f);
    }
    public void ScoreTimerTimeoutHandler()
    {
        score += 1;

        _ui.Call("UpdateScore", score);
    }

    public void StartTimerTimeoutHandler()
    {
        GD.Print("start timer timeout");
        _scoreTimer.Start();
        _obstacleTimer.Start();
    }

}
