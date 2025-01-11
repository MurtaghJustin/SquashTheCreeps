using Godot;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

public partial class Mob : CharacterBody3D
{
	[Export]
	public int MinSpeed { get; set; } = 10;
	[Export]
	public int MaxSpeed { get; set; } = 18;

	[Signal]
	public delegate void SquashedEventHandler();

	private AnimationPlayer AnimationPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public void Initialize(Vector3 startPosition, Vector3 playerPosition)
	{
		// We position the mob by placing it at startPosition
		// and rotate it towards playerPosition, so it looks at the player.
		LookAtFromPosition(startPosition, playerPosition, Vector3.Up);
		// Rotate this mob randomly within range of -45 and +45 degrees,
		// so that it doesn't move directly towards the player.
		RotateY((float)GD.RandRange(-Mathf.Pi / 4d, Mathf.Pi / 4d));

		int randomSpeed = GD.RandRange(MinSpeed, MaxSpeed);
		Velocity = Vector3.Forward * randomSpeed;
		Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);
		AnimationPlayer.SpeedScale = randomSpeed / MinSpeed;
	}

	public void OnVisibilityNotifierScreenExited()
	{
		QueueFree();
	}

	public void Squash()
	{
		EmitSignal(SignalName.Squashed);
		QueueFree();
	}
}
