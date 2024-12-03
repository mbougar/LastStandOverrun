using Godot;
using System;

public partial class UiGame : CanvasLayer
{
	private Global _global;
	private Label _waveLabel;
	private Label _killsLabel;
	private TextureProgressBar _hpBar;
	private int _kills = 0;
	private int _wave = 0;
	private int _hp = 100;
	
	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");
		_waveLabel = GetNode<Label>("Wave");
		_killsLabel = GetNode<Label>("Kills");
		_hpBar = GetNode<TextureProgressBar>("Hp");
		_hpBar.Value = 100;
	}

	public override void _Process(double delta)
	{
		if (_global.CurrentWave != _wave)
		{
			_wave = _global.CurrentWave;
			_waveLabel.Text = ($"Wave {_wave}");
		}
		
		if (_global.CurrentKills != _kills)
		{
			_kills = _global.CurrentKills;
			_killsLabel.Text = ($"Kills: {_kills}");
		}
		
		if (_global.CurrentPlayerHp != _hp)
		{
			_hp = _global.CurrentPlayerHp;
			_hpBar.Value = _hp;
			_hpBar.MaxValue = 100;
		}
	}
}
