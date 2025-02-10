using CozyGame.Entity;
using Godot;
using Tree = CozyGame.Entity.Tree;

public partial class TreeSpawner : RandomSpawner<Tree>
{
	[Export]
	public PackedScene EntityScene
	{
		get => _entityScene;
		set => _entityScene = value;
	}

	[Export]
	public Vector2I SpawnPeriod
	{
		get => _spawnPeriod;
		set => _spawnPeriod = value;
	}

	[Export]
	public Rect2 SpawnArea
	{
		get => _spawnArea;
		set => _spawnArea = value;
	}
}
