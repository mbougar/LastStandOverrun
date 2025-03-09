using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	[Export] private int speed = 300;
	private int hp = 100;
	private int resistance = 0;
	private Vector2 currentVelocity;
	
	private string texturePath1 = "res://assets/items/zombieFootIcon.png";
	private string texturePath2 = "res://assets/items/heartIcon.png";
	private string texturePath3 = "res://assets/items/zombieHandIcon.png";
	private Texture2D[] _textures = new Texture2D[3];

	private AnimatedSprite2D _animatedSprite;
	private AudioStreamPlayer2D _audioPlayer;
	private Timer _footstepTimer;
	private Global _global;
	private Area2D _hitboxArea;
	private List<Node> enemiesInside = new List<Node>();
	private Inventory _inventory;

	public override void _Ready()
	{
		_textures[0] = (Texture2D)ResourceLoader.Load(texturePath1);
		_textures[1] = (Texture2D)ResourceLoader.Load(texturePath2);
		_textures[2] = (Texture2D)ResourceLoader.Load(texturePath3);
		
		_inventory = GetNode<Inventory>("/root/BaseCamp/InventoryCanvas/Inventory");
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
				TakeDamage(1);
				_global.GetCurrentHp(hp);
			}
		}

		if (hp <= 0)
		{
			SubmitScore(_global.CurrentKills * 100);
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
		hp -= Mathf.RoundToInt(damage * (100f / (100f + resistance)));
		_global.GetCurrentHp(hp);
	}

	private void OnEnemyExitedArea(Node body)
	{
		if (body is Enemy)
		{
			enemiesInside.Remove(body);
		}
	}

	public void IncreaseSpeed()
	{
		speed += 10;
		_inventory.AddInventoryItem(
			new Item(
				"Zombie foot",
				_textures[0],
				1
			)
		);
	}
	
	public async void SubmitScore(int score)
	{
		if (string.IsNullOrEmpty(_global.Username))
		{
			GD.Print("No user logged in. Cannot submit score.");
			return;
		}
		await _global.SubmitScoreAsync(_global.Username, score);
	}


	public void IncreaseResistance()
	{
		resistance += 10;
		_inventory.AddInventoryItem(
			new Item(
				"Zombie heart",
				_textures[1],
				1
			)
		);
	}
	
	// Ahora mismo no hace nada todavia  a parte de darte el objeto en el inventario
	public void IncreaseDamage()
	{
		_inventory.AddInventoryItem(new Item(
				"Zombie arm",
				_textures[2],
				1
			)
		);
	}
}
