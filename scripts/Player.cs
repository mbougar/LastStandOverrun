using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	[Export]
	private int speed = 300;
	private int hp = 100;
	private Vector2 currentVelocity;

	private AnimatedSprite2D _animatedSprite;
	private AudioStreamPlayer2D _audioPlayer;
	private Timer _footstepTimer;
	private Global _global;
	private Area2D _hitboxArea;
	private List<Node> enemiesInside = new List<Node>();

	public override void _Ready()
	{
		_hitboxArea = GetNode<Area2D>("HitboxArea");
		_global = GetNode<Global>("/root/Global");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_footstepTimer = GetNode<Timer>("FootstepTimer");
	}

	public override void _Process(double delta)
	{

		handleInput();

		Velocity = currentVelocity;
		MoveAndSlide();

		if (currentVelocity.Length() > 0.0)
		{
			_animatedSprite.Play("run");
			if (_footstepTimer.TimeLeft <= 0)
			{
				_audioPlayer.PitchScale = (float)GD.RandRange(0.8, 1.2);
				_audioPlayer.Play();
				_footstepTimer.Start();
			}
		}
		else
		{
			_animatedSprite.Play("idle");
		}
		
		foreach (var enemy in enemiesInside)
		{
			if (enemy is Enemy)
			{
				hp -= 1;
				_global.GetCurrentHp(hp);
			}
		}
		
		if (hp <= 0)
		{
			_global.GotoScene("res://scenes/menu/MainMenu.tscn");
		}
	}

	private void handleInput()
	{
		currentVelocity = Input.GetVector("left", "right", "up", "down");
		currentVelocity *= speed;
	}
	
	private void OnEnemyEnteredArea(Node body)
	{
		if (body is Enemy)
		{
			enemiesInside.Add(body);
		}
	}
	
	public void TakeDamage(int damage)
	{
		hp -= damage;
		_global.GetCurrentHp(hp);
	}
	
	private void OnEnemyExitedArea(Node body)
	{
		if (body is Enemy)
		{
			enemiesInside.Remove(body);
		}
	}
}
