using Godot;
using System;

public class UI : CanvasLayer
{
    // signals
    [Signal] delegate void StartGame();

    // nodes
    private Label _scoreLabel;
    private Label _message;
    private Button _startButton;
    private Timer _messageTimer;
    public override void _Ready()
    {
        // assign variables
        _scoreLabel = GetNode<Label>("ScoreLabel");
        _message = GetNode<Label>("Message");
        _startButton = GetNode<Button>("StartButton");
        _messageTimer = GetNode<Timer>("MessageTimer");

        // signals
        _startButton.Connect("pressed", this, nameof(StartButtonPressedHandler));
        _messageTimer.Connect("timeout", this, nameof(MessageTimerTimeoutHandler));
    }
    public void ShowMessage(string text)
    {
        _message.Text = text;
        _message.Show();
        _messageTimer.Start();
    }
    public async void ShowGameOver()
    {
        ShowMessage("Game Over");

        // yield until timeout signal is fired
        await ToSignal(_messageTimer, "timeout");

        _message.Text = "Po!";
        _message.Show();

        await ToSignal(GetTree().CreateTimer(1), "timeout");

        _startButton.Show();
    }
    public void UpdateScore(int score)
    {
        _scoreLabel.Text = score.ToString();
    }
    public void StartButtonPressedHandler()
    {
        _startButton.Hide();
        EmitSignal(nameof(StartGame));
    }
    public void MessageTimerTimeoutHandler()
    {
        GD.Print("message timeout: hiding message");
        _message.Hide();
    }
}
