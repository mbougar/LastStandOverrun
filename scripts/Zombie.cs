using Godot;

public partial class Zombie : Enemy
{
	private const float Speed = 80f;
	private Node2D _player;
	private NavigationAgent2D _navAgent;
	private AnimatedSprite2D _sprite;
	private bool _isDying = false;
	private bool _hasDied = false;
	public override int Health { get; protected set; } = 10;
	
	private HordeManager _hordeManager;

	public override void _Ready()
	{
		var global = GetNode<Global>("/root/Global");
		int currentWave = global.CurrentWave;
		Health = (int)(BaseHealth * (1.0f + (currentWave - 1) * 0.5f));
		_navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_player = GetNode<Node2D>("/root/BaseCamp/Player");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_hordeManager = GetNode<HordeManager>("/root/BaseCamp/HordeManager");

		_sprite.AnimationFinished += OnAnimationFinished;
	}

	public override void _Process(double delta)
	{
		if (_isDying) return;

		Vector2 direction = ToLocal(_navAgent.GetNextPathPosition()).Normalized();
		Velocity = direction * Speed;
		if (Velocity.X != 0)
		{
			_sprite.Scale = new Vector2(Mathf.Sign(Velocity.X), _sprite.Scale.Y);
		}
		MoveAndSlide();
	}

	public void MakePath()
	{
		if (!_isDying && _player != null)
		{
			_navAgent.TargetPosition = _player.GlobalPosition;
		}
	}

	private void OnTimerTimeout()
	{
		if (!_isDying)
		{
			MakePath();
		}
	}

	public override void TakeDamage(int amount)
	{
		if (_isDying) return;

		Health -= amount;

		if (Health <= 0)
		{
			_sprite.Stop();
			_isDying = true;
			_sprite.Play("death");
		}
	}

	private void OnAnimationFinished()
	{
		if (_isDying && _sprite.Animation == "death" && !_hasDied)
		{
			_hasDied = true;
			Die();
		}
	}

	protected override void Die()
	{
		_hordeManager.RegisterEnemyKilled();
		QueueFree();
	}
}
