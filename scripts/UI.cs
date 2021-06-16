using Godot;
using System;

public class UI : CanvasLayer
{
	// signals
	[Signal] delegate void startGame();

	// nodes
	private Label scoreLabel;
	private Label message;
	private Button startButton;
	private Timer messageTimer;
	public override void _Ready()
	{
		// assign variables
		scoreLabel = GetNode<Label>("ScoreLabel");
		message = GetNode<Label>("Message");
		startButton = GetNode<Button>("StartButton");
		messageTimer = GetNode<Timer>("MessageTimer");

		// signals
		startButton.Connect("pressed", this, nameof(onStartButtonPressed));
		messageTimer.Connect("timeout", this, nameof(onMessageTimerTimeout));
	}
	// public override void _Input(InputEvent inputEvent)
	// {
	// 	if (inputEvent.IsActionPressed("ui_accept"))
	// 	{
	// 		onStartButtonPressed();
	// 	}
	// }
	public void showMessage(string text)
	{
		message.Text = text;
		message.Show();
		messageTimer.Start();
	}
	public async void showGameOver()
	{
		showMessage("Game Over");

		// yield until timeout signal is fired
		await ToSignal(messageTimer, "timeout");

		message.Text = "Po!";
		message.Show();
		
		await ToSignal(GetTree().CreateTimer(1), "timeout");

		startButton.Show();
	}
	public void updateScore(int score)
	{
		scoreLabel.Text = score.ToString();
	}
	public void onStartButtonPressed()
	{
		startButton.Hide();
		EmitSignal("startGame");
	}
	public void onMessageTimerTimeout()
	{
		GD.Print("message timeout: hiding message");
		message.Hide();
	}
}
