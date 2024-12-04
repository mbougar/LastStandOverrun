using Godot;
using System;

public partial class MainMenu : Control
{	
	public void OnPlayButtonPressed()
	{
		var global = GetNode<Global>("/root/Global");
		global.GotoScene("res://scenes/game/levels/BaseCamp.tscn");
	}
	
	public void OnQuitButtonPressed() {
		GetTree().Quit();
	}
}
