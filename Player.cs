using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public int Speed { get; set; } = 14;
	[Export]
	public int FallAcceleration { get; set; } = 75;
	
	private Vector3 _targetVelocity = Vector3.Zero;

	private Node3D Pivot;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pivot = GetNode<Node3D>("Pivot");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var d = (float)delta;
		var direction = Vector3.Zero;

		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1.0f;
		}
		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 1.0f;
		}
		if (Input.IsActionPressed("move_back"))
		{
			direction.Z += 1.0f;
		}

		// Normalize so speed is 1 instead of sqrt(2)
		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			Pivot.Basis = Basis.LookingAt(direction);
		}

		// Apply ground velocity
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		// Vertical
		if (!IsOnFloor())
		{
			_targetVelocity.Y -= FallAcceleration * d;
		}

		Velocity = _targetVelocity;
		MoveAndSlide();
	}
}
