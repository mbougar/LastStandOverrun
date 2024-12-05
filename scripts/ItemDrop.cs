using Godot;
using System;

public partial class ItemDrop : Node2D
{
	[Export] public Sprite2D sprite;
	[Export] public Area2D area;
	private int _randomNum;
	
	private string texturePath1 = "res://assets/items/zombieFootIconTransparent.png";
	private string texturePath2 = "res://assets/items/heartIconTransparent.png";
	private string texturePath3 = "res://assets/items/zombieHandIconTransparent.png";

	public override void _Ready()
	{
		
		if (sprite != null && area != null)
		{
			Texture2D[] textures = new Texture2D[3];
			textures[0] = (Texture2D)ResourceLoader.Load(texturePath1);
			textures[1] = (Texture2D)ResourceLoader.Load(texturePath2);
			textures[2] = (Texture2D)ResourceLoader.Load(texturePath3);

			RandomNumberGenerator rng = new RandomNumberGenerator();
			_randomNum = rng.RandiRange(0, textures.Length - 1);
			

			sprite.Texture = textures[_randomNum];
		}
		else
		{
			QueueFree();
		}
	}

	private void OnAreaEntered(Node body)
	{
		if (body is Player player)
		{
			switch (_randomNum)
			{
				case 0:
					player.IncreaseSpeed();
					break;
				case 1:
					player.IncreaseResistance();
					break;
				case 2:
					player.IncreaseDamage();
					break;
			}
			QueueFree();
		}
	}

	public override void _Process(double delta)
	{
	}
}
