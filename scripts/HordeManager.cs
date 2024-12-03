using Godot;
using System;
using System.Collections.Generic;

public partial class HordeManager : Node2D
{
	private List<PackedScene> _enemyScenes = new List<PackedScene>();

	private Timer _spawnTimer;
	private Timer _roundDelayTimer;
	private Node2D _spawnArea;
	private Global _global;

	private Node _enemiesParent;
	private int _currentRound = 1;
	private int _enemiesToSpawn;
	private int _enemiesKilled;

	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");
		_enemyScenes.Add(GD.Load<PackedScene>("res://scenes/game/characters/Zombie.tscn"));
		_enemyScenes.Add(GD.Load<PackedScene>("res://scenes/game/characters/ZombieBig.tscn"));

		_spawnTimer = GetNode<Timer>("SpawnTimer");
		_roundDelayTimer = GetNode<Timer>("RoundDelayTimer");
		_spawnArea = GetNode<Node2D>("SpawnArea");

		_enemiesParent = GetNode("Enemies");

		StartNewRound();
	}

	private void StartNewRound()
	{
		_global.IncrementWave();
		_enemiesToSpawn = 8 + (_currentRound - 1) * 8;
		_enemiesKilled = 0;

		_spawnTimer.Start();
	}

	private void OnSpawnEnemy()
	{
		if (_enemiesToSpawn <= 0)
		{
			_spawnTimer.Stop();
			return;
		}

		_enemiesToSpawn--;

		int randomIndex = (int)(GD.Randf() * _enemyScenes.Count);
		PackedScene selectedEnemyScene = _enemyScenes[randomIndex];

		Node2D enemy = selectedEnemyScene.Instantiate<Node2D>();
		_enemiesParent.AddChild(enemy);

		Vector2 spawnPosition = GetRandomSpawnPosition();
		enemy.Position = spawnPosition;
	}

	private Vector2 GetRandomSpawnPosition()
	{
		Area2D area2D = _spawnArea.GetNode<Area2D>("Area2D");
		CollisionShape2D collisionShape = area2D.GetNode<CollisionShape2D>("CollisionShape2D");

		RectangleShape2D rectangleShape = collisionShape.Shape as RectangleShape2D;

		if (rectangleShape == null)
		{
			return Vector2.Zero;
		}

		Vector2 areaPosition = _spawnArea.GlobalPosition;
		Vector2 rectSize = rectangleShape.Size;

		float x = (float)GD.RandRange(areaPosition.X - rectSize.X / 2, areaPosition.X + rectSize.X / 2);
		float y = (float)GD.RandRange(areaPosition.Y - rectSize.Y / 2, areaPosition.Y + rectSize.Y / 2);

		return new Vector2(x, y);
	}

	public void RegisterEnemyKilled()
	{
		_enemiesKilled++;
		_global.IncrementKills();

		if (_enemiesKilled >= 8 + (_currentRound - 1) * 8)
		{
			_currentRound++;
			_enemiesKilled = 0;
			_roundDelayTimer.Start();
		}
	}

	private void OnRoundDelayTimeout()
	{
		StartNewRound();
	}
}
