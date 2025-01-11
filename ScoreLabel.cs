using Godot;
using System;

public partial class ScoreLabel : Label
{
	private int _score = 0;
	private int _highScore = 0;
	private string ScoreText
	{
		get
		{
			return $"Squishes: {_score:D2} | Highscore: {_highScore:D2}";
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnMobSquashed()
	{
		UpdateScore(_score + 1);
	}

	public void UpdateScore(int score)
	{
		_score = score;
		if (_score > _highScore)
		{
			_highScore = _score;
		}
		Text = ScoreText;
	}
}
