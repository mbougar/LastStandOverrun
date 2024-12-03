using Godot;
using System;
using System.Linq;

public partial class Drone : CharacterBody2D
{
	[Export]
	private float followSpeed = 200.0f;
	[Export]
	private float followDistance = 100.0f;
	[Export]
	private float shootRadius = 300.0f;

	private Node2D player;
	private Vector2 velocity;
	private AnimatedSprite2D sprite;
	private Timer _fireCooldownTimer;
	private AudioStreamPlayer2D _audioPlayer;
	private Marker2D _muzzle;
	private Area2D _shootingArea;
	private bool _canFire = true;

	private PackedScene bulletScene;

	public override void _Ready()
	{
		_audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_shootingArea = GetNode<Area2D>("Area2D");
		_muzzle = GetNode<Marker2D>("Muzzle");
		bulletScene = (PackedScene)ResourceLoader.Load("res://scenes/Bullet.tscn");
		_fireCooldownTimer = GetNode<Timer>("FireCooldownTimer");
		player = GetParent().GetNode<Node2D>("Player");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _Process(double delta)
	{
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

		Enemy nearestEnemy = GetNearestEnemy(shootRadius);
		if (nearestEnemy != null)
		{
			Vector2 directionToEnemy = nearestEnemy.GlobalPosition - GlobalPosition;
			float angleToEnemy = directionToEnemy.Angle();

			ShootBulletAtEnemy(nearestEnemy);
		}

		if (velocity.X != 0)
		{
			sprite.Scale = new Vector2(Mathf.Sign(velocity.X), sprite.Scale.Y);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private Enemy GetNearestEnemy(float radius)
	{
		var overlappingBodies = _shootingArea.GetOverlappingBodies();
		Enemy targetEnemy = null;
		float closestDistance = radius;  // Start with the radius as the max allowed distance

		// Iterate through all overlapping bodies
		foreach (var body in overlappingBodies)
		{
			// Ensure the body is an instance of Enemy
			if (body is Enemy enemy)
			{
				// Calculate the distance from the shooting area to this enemy
				float distance = _shootingArea.GlobalPosition.DistanceTo(enemy.GlobalPosition);

				// If the enemy is within the range and closer than the previous one, update the target
				if (distance <= radius && distance < closestDistance)
				{
					targetEnemy = enemy;
					closestDistance = distance;
				}
			}
		}

		return targetEnemy;
	}

	private void ResetFire()
	{
		_canFire = true;
	}

	public void ShootBulletAtEnemy(Enemy enemy)
	{
		if (_canFire)
		{
			_audioPlayer.PitchScale = (float)GD.RandRange(0.95, 1.05);
			_audioPlayer.Play();

			Bullet bulletInstance = (Bullet)bulletScene.Instantiate();
			GetTree().Root.AddChild(bulletInstance);

			bulletInstance.GlobalPosition = _muzzle.GlobalPosition;

			Vector2 directionToEnemy = enemy.GlobalPosition - GlobalPosition;
			float spreadAngle = (float)GD.RandRange(-0.1, 0.1);
			bulletInstance.GlobalRotation = directionToEnemy.Angle() + spreadAngle;

			_canFire = false;
			_fireCooldownTimer.Start();
		}
	}
}
