using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }

	private Player Player;
	private PathFollow3D MobSpawnLocation;
	private Timer MobTimer;
	private ScoreLabel UIScoreLabel;
	private Control UIRetry;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Setup node references
		Player = GetNode<Player>("Player");
		MobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		MobTimer = GetNode<Timer>("MobTimer");
		UIScoreLabel = GetNode<ScoreLabel>("UI/ScoreLabel");
		UIRetry = GetNode<Control>("UI/Retry");

		UIRetry.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnMobTimerTimeout()
	{
		var mob = MobScene.Instantiate<Mob>();
		var spawnLoc = MobSpawnLocation;
		spawnLoc.ProgressRatio = GD.Randf();

		Vector3 playerPosition = Player.Position;
		mob.Initialize(spawnLoc.Position, playerPosition);
		mob.Squashed += UIScoreLabel.OnMobSquashed;
		AddChild(mob);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept") && UIRetry.Visible)
			GetTree().ReloadCurrentScene();
	}

	private void OnPlayerHit()
	{
		MobTimer.Stop();
		UIRetry.Show();
	}
}
