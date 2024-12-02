using Godot;

public partial class Bullet : Node2D
{
	[Export] private int speed = 100;
	[Export] private int damage = 20;

	private Area2D _area;

	public override void _Ready()
	{
		_area = GetNode<Area2D>("Area2D");
		_area.Connect("body_entered", Callable.From((Node body) => OnBodyEntered(body)));
	}

	public override void _Process(double delta)
	{
		GlobalPosition += Transform.X * speed * (float)delta;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Enemy enemy)
		{
			enemy.TakeDamage(damage);
			QueueFree();
		}
	}

	public void _on_visible_on_screen_enabler_2d_screen_exited() 
	{
		QueueFree();
	}
}
