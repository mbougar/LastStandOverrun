using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Gun : Node2D
{
	private Node2D player;

	private static readonly NodePath MuzzlePath = new NodePath("Muzzle");
	private Marker2D _muzzle;
	[Export]
	public float FireRate = 0.2f;
	private bool _canFire = true;
	private Timer _fireCooldownTimer;
	
	private PackedScene bulletScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent<Node2D>();
		_muzzle = GetNode<Marker2D>(MuzzlePath);
		bulletScene = (PackedScene)ResourceLoader.Load("res://scenes/bullet.tscn");
		_fireCooldownTimer = new Timer();
		_fireCooldownTimer.OneShot = true;
		_fireCooldownTimer.WaitTime = FireRate;
		AddChild(_fireCooldownTimer);
		_fireCooldownTimer.Connect("timeout", new Callable(this, nameof(ResetFire)));
	}

	public override void _Process(double delta)
	{
		LookAt(GetGlobalMousePosition());

		float rotationDegreesWrapped = RotationDegrees % 360;
		if (rotationDegreesWrapped < 0)
		{
			rotationDegreesWrapped += 360;
		}

		if (rotationDegreesWrapped > 90 && rotationDegreesWrapped < 270)
		{
			Scale = new Vector2(Scale.X, -1);
		}
		else
		{
			Scale = new Vector2(Scale.X, 1);
		}

		if (player != null)
		{
			player.Scale = new Vector2(Scale.Y, player.Scale.Y);
		}
		
		if (Input.IsActionPressed("shoot"))
		{
			if (_canFire)
			{
				ShootBullet();
				_canFire = false;
				_fireCooldownTimer.Start();
			}
		}
	}
	
	private void ResetFire()
	{
		_canFire = true;
	}
	
	public void ShootBullet()
	{
		Bullet bulletInstance = (Bullet)bulletScene.Instantiate();
		GetTree().Root.AddChild(bulletInstance);

		bulletInstance.GlobalPosition = _muzzle.GlobalPosition;
		bulletInstance.GlobalRotation = GlobalRotation;
	}
}
