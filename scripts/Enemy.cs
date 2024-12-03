using Godot;

public partial class Enemy : CharacterBody2D
{
	public virtual int Health { get; protected set; } = 10;
	public virtual int BaseHealth { get; protected set; } = 10;

	public virtual void TakeDamage(int amount)
	{
		Health -= amount;

		if (Health <= 0)
			Die();
	}

	protected virtual void Die()
	{
		QueueFree();
	}
}
