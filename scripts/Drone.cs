using Godot;
using System;

public partial class Drone : CharacterBody2D
{
	[Export]
	private float followSpeed = 200.0f;
	[Export]
	private float followDistance = 100.0f;

	private Node2D player;
	private Vector2 velocity;
	private AnimatedSprite2D sprite;

	public override void _Ready()
	{
		player = GetParent().GetNode<Node2D>("Player");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (player == null)
		{
			GD.PrintErr("Player no encontrado en el nodo padre.");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (player == null) return;

		Vector2 directionToPlayer = (player.GlobalPosition - GlobalPosition).Normalized();
		float distanceToPlayer = (player.GlobalPosition - GlobalPosition).Length();

		if (distanceToPlayer > followDistance)
		{
			velocity = directionToPlayer * followSpeed;
		}
		else
		{
			velocity = Vector2.Zero;
		}

		if (velocity.X != 0)
		{
			sprite.Scale = new Vector2(-Mathf.Sign(velocity.X), sprite.Scale.Y);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
