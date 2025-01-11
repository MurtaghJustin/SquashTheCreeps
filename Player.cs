using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public int Speed { get; set; } = 14;
	[Export]
	public int FallAcceleration { get; set; } = 75;
	[Export]
	public int JumpImpulse { get; set; } = 20;
	[Export]
	public int BounceImpulse { get; set; } = 16;

	[Signal]
	public delegate void HitEventHandler();
	
	private Vector3 _targetVelocity = Vector3.Zero;
	private Node3D Pivot;

	private bool _running = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pivot = GetNode<Node3D>("Pivot");
	}

	public void Start()
	{
		Position = new Vector3(0, 0, 0);
		_running = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (!_running)
		{
			return;
		}

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
		else if (Input.IsActionPressed("jump"))
		{
			_targetVelocity.Y = JumpImpulse;
		}

		var slideCollisionCount = GetSlideCollisionCount();
		for (int i = 0; i < slideCollisionCount; i++)
		{
			var collision = GetSlideCollision(i);

			if (collision.GetCollider() is Mob mob)
			{
				// Check if we're hitting it from above
				if (Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
				{
					mob.Squash();
					_targetVelocity.Y = BounceImpulse;
					break;
				}
			}
		}

		Velocity = _targetVelocity;
		MoveAndSlide();
	}

	private void Die()
	{
		if (!_running)
			return;

		EmitSignal(SignalName.Hit);
		_running = false;
	}

	private void OnMobDetectorBodyEntered(Node3D body)
	{
		Die();
	}
}
