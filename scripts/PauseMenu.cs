using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	private bool _paused = false;
	private Global _global;
	
	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			Pause();
		}
	}
	
	public void Pause()
	{
		if (_paused)
		{
			Hide();
			_paused = false;
			GetTree().Paused = false;
		}
		else
		{
			Show();
			_paused = true;
			GetTree().Paused = true;
		}
	}
	
	private void OnResumePressed()
	{
		Pause();
	}
	
	private void OnMainMenuPressed()
	{
		Pause(); // Importante quitar la pausa o el menu principal no funcionara
		_global.GotoScene("res://scenes/menu/MainMenu.tscn");
	}
	
	private void OnQuitPressed()
	{
		Pause();
		GetTree().Quit();
	}
}
