using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }

	private Player Player;
	private PathFollow3D MobSpawnLocation;
	private Timer MobTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player = GetNode<Player>("Player");
		MobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		MobTimer = GetNode<Timer>("MobTimer");
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
		AddChild(mob);
	}

	private void OnPlayerHit()
	{
		MobTimer.Stop();
	}
}
