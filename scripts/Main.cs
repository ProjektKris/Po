using Godot;
using System;
using Game;

public class Main : Node2D
{
    [Export] public PackedScene GroundObstacleScene;

    private int _score;

    private Player _player;
    private UI _ui;
    private Timer _scoreTimer;
    private Timer _obstacleTimer;
    private Timer _startTimer;
    private Position2D _playerPosition;
    private Position2D _obstacleStartPos;

    public override void _Ready()
    {
        _player = GetNode<Player>("Player");
        _ui = GetNode<UI>("UI");
        _scoreTimer = GetNode<Timer>("ScoreTimer");
        _obstacleTimer = GetNode<Timer>("ObstacleTimer");
        _startTimer = GetNode<Timer>("StartTimer");
        _playerPosition = GetNode<Position2D>("PlayerPosition");
        _obstacleStartPos = GetNode<Position2D>("GroundObstacleStart");

        // signals
        _ui.Connect(nameof(UI.StartGame), this, nameof(NewGame));
        _player.Connect(nameof(Player.Hit), this, nameof(GameOver));
        _obstacleTimer.Connect("timeout", this, nameof(ObstacleTimerTimeoutHandler));
        _scoreTimer.Connect("timeout", this, nameof(ScoreTimerTimeoutHandler));
        _startTimer.Connect("timeout", this, nameof(StartTimerTimeoutHandler));
    }
    public void GameOver()
    {
        // stop score timer
        _scoreTimer.Stop();

        // stop obstacle timer
        _obstacleTimer.Stop();

        // show hud game over
        _ui.ShowGameOver();

        // remove old enemies
        GetTree().CallGroup("obstacle", "queue_free");

        // stop music
    }
    public void NewGame()
    {
        // reset score
        _score = 0;

        // set player position
        _player.Start(_playerPosition.Position);

        // start countdown timer
        _startTimer.Start();

        // update hud score
        _ui.UpdateScore(_score);

        // show message get ready
        _ui.ShowMessage("Get Ready");

        // play music
    }
    public void ObstacleTimerTimeoutHandler()
    {
        GD.Print("spawning ground obstacle");

        GroundObstacle newObstacle = (GroundObstacle)GroundObstacleScene.Instance();
        AddChild(newObstacle);
        newObstacle.Position = _obstacleStartPos.Position;

        // rng for next timeout
        _obstacleTimer.WaitTime = (float)GD.RandRange(1.5f, 3f);
    }
    public void ScoreTimerTimeoutHandler()
    {
        _score += 1;
        _ui.UpdateScore(_score);
    }
    public void StartTimerTimeoutHandler()
    {
        GD.Print("start timer timeout");
        _scoreTimer.Start();
        _obstacleTimer.Start();
    }

}
