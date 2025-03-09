using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class MainMenu : Control
{
	private Global global;
	private ItemList leaderboardList;

	public override async void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		leaderboardList = GetNode<ItemList>("Leaderboard");

		await LoadLeaderboard();
	}

	private async Task LoadLeaderboard()
	{
		List<LeaderboardEntry> leaderboardData = await global.GetLeaderboardAsync();

		leaderboardList.Clear();

		// Add leaderboard entries
		foreach (var entry in leaderboardData)
		{
			leaderboardList.AddItem($"{entry.Username} - {entry.HighestScore}");
			GD.Print($"{entry.Username} - {entry.HighestScore}");
		}
	}

	public void OnPlayButtonPressed()
	{
		global.GotoScene("res://scenes/game/levels/BaseCamp.tscn");
	}

	public void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
