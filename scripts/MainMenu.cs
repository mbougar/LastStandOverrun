using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnPlayButtonPressed()
	{
		var global = GetNode<Global>("/root/Global");
		global.GotoScene("res://scenes/game/levels/base_camp.tscn");
	}
}
