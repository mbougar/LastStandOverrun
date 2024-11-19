using Godot;
using System;

public partial class Bullet : Node2D
{

	[Export]
	private int speed = 100;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Move the bullet in the direction it’s facing
		GlobalPosition += Transform.X * speed * (float)delta;
	}

	public void _on_visible_on_screen_enabler_2d_screen_exited() {
		QueueFree();
	}
}
