using Godot;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public partial class Global : Node
{
	public Node CurrentScene { get; set; }
	public int CurrentWave { get; set; } = 0;
	public int CurrentKills { get; set; } = 0;
	public int CurrentPlayerHp { get; set; } = 100;
	public string Username { get; private set; }

	private readonly System.Net.Http.HttpClient _httpClient;
	private const string ApiUrl = "https://api-godot-psp.onrender.com/api/user";

	public Global()
	{
		_httpClient = new System.Net.Http.HttpClient();
	}

	public override void _Ready()
	{
		Viewport root = GetTree().Root;
		CurrentScene = root.GetChild(-1);
	}

	public void GotoScene(string path)
	{
		CallDeferred(MethodName.DeferredGotoScene, path);
	}

	public void DeferredGotoScene(string path)
	{
		CurrentScene.Free();

		var nextScene = GD.Load<PackedScene>(path);
		
		CurrentScene = nextScene.Instantiate();
		GetTree().Root.AddChild(CurrentScene);
		GetTree().CurrentScene = CurrentScene;
	}

	public void IncrementWave()
	{
		CurrentWave++;
	}
	
	public void IncrementKills()
	{
		CurrentKills++;
	}

	public void GetCurrentHp(int hp)
	{
		CurrentPlayerHp = hp;
	}

	public void ResetGame()
	{
		CurrentWave = 0;
		CurrentKills = 0;
		CurrentPlayerHp = 100;
	}

	public async Task RegisterUserAsync(string username, string password)
	{
		var user = new { Username = username, PasswordHash = password };
		var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

		var response = await _httpClient.PostAsync($"{ApiUrl}/register", jsonContent);
		if (response.IsSuccessStatusCode)
		{
			Username = username;
			GD.Print("User registered successfully");
		}
		else
		{
			GD.Print("Failed to register user");
		}
	}

	public async Task LoginUserAsync(string username, string password)
	{
		var user = new { Username = username, PasswordHash = password };
		var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

		var response = await _httpClient.PostAsync($"{ApiUrl}/login", jsonContent);
		if (response.IsSuccessStatusCode)
		{
			Username = username;
			GD.Print("Login successful");
		}
		else
		{
			GD.Print("Failed to login");
		}
	}

	public async Task SubmitScoreAsync(string username, int score)
	{
		var userScore = new { Username = username, HighestScore = score };
		var jsonContent = new StringContent(JsonSerializer.Serialize(userScore), Encoding.UTF8, "application/json");

		var response = await _httpClient.PostAsync($"{ApiUrl}/submit-score", jsonContent);
		if (response.IsSuccessStatusCode)
		{
			GD.Print("Score submitted successfully");
		}
		else
		{
			GD.Print("Failed to submit score");
		}
	}

	public async Task<List<LeaderboardEntry>> GetLeaderboardAsync()
	{
		try
		{
			HttpResponseMessage response = await _httpClient.GetAsync($"{ApiUrl}/leaderboard");
			response.EnsureSuccessStatusCode();

			string json = await response.Content.ReadAsStringAsync();

			List<LeaderboardEntry>? leaderboard = JsonSerializer.Deserialize<List<LeaderboardEntry>>(json);
			return leaderboard ?? new List<LeaderboardEntry>();
		}
		catch (Exception ex)
		{
			GD.PrintErr("Failed to fetch leaderboard:", ex.Message);
			return new List<LeaderboardEntry>();
		}
	}

	public override void _ExitTree()
	{
		_httpClient.Dispose();
	}
}

public class LeaderboardEntry
{
	[JsonPropertyName("username")]
	public string Username { get; set; }

	[JsonPropertyName("highestScore")]
	public int HighestScore { get; set; }
}
