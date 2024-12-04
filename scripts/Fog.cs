using Godot;
using System;

public partial class Fog : Node2D
{
	private Player playerInside = null;
	private bool _canTakeDamage = true;
	private Timer _damageTimer;

	public override void _Ready()
	{
		_damageTimer = GetNode<Timer>("DamageTimer");
	}

	public override void _Process(double delta)
	{
		if (playerInside != null && _canTakeDamage)
		{
			playerInside.TakeDamage(40);
			_canTakeDamage = false;
			_damageTimer.Start();
		}
	}
	
	private void OnAreaEntered(Node body)
	{
		if (body is Player player)
		{
			playerInside = player;
		}
	}
	
	private void OnAreaExited(Node body)
	{
		if (body is Player)
		{
			playerInside = null;
		}
	}
	
	private void ResetDamage()
	{
		_canTakeDamage = true;
	}
}
