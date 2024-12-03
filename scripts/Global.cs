using Godot;

public partial class Global : Node
{
	public Node CurrentScene { get; set; }
	public int CurrentWave { get; set; } = 0;
	public int CurrentKills { get; set; } = 0;
	public int CurrentPlayerHp { get; set; } = 0;

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
}
