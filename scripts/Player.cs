using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	private int speed = 300;
	private Vector2 currentVelocity;

	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		handleInput();

		Velocity = currentVelocity;
		MoveAndSlide();

		if (currentVelocity.Length() > 0.0)
		{
			_animatedSprite.Play("run");
		}
		else
		{
			_animatedSprite.Play("idle");
		}
	}

	private void handleInput()
	{
		currentVelocity = Input.GetVector("left", "right", "up", "down");
		currentVelocity *= speed;
	}
}
