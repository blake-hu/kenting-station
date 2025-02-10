using System;
using CozyGame.scene;
using Godot;

public partial class RandomSpawner : Node2D
{
	private readonly Random _rng = new();
	private float _nextSpawnPeriod;
	private float _tickCounter;

	[Export] public PackedScene EntityScene;
	[Export] public Rect2 SpawnArea;
	[Export] public Vector2I SpawnPeriod;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ValidateSpawnPeriod();
		_nextSpawnPeriod = GetSpawnPeriod();
	}

	private void ValidateSpawnPeriod()
	{
		var first = SpawnPeriod.X;
		var second = SpawnPeriod.Y;
		if (first > second)
			throw new ArgumentException(
				$"First value ({first}) of SpawnPeriodRange must be smaller than second value ({second}).");
		if (first < 0.0f)
			throw new ArgumentException($"First value ({first}) of SpawnPeriodRange must be a positive value.");
	}


// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_tickCounter >= _nextSpawnPeriod)
		{
			Spawn();
			_nextSpawnPeriod = GetSpawnPeriod();
			_tickCounter = 0;
		}
		else
		{
			_tickCounter++;
		}
	}

	private void Spawn()
	{
		var entity = EntityScene.Instantiate<Node2D>();
		var spawnLocation = GetSpawnLocation();
		entity.Position = spawnLocation;
		entity.Name = new EntityId(entity.Name);
		MobContainer.Instance.AddChild(entity);
		GD.Print($"Spawned {entity.Name} at location {spawnLocation} after {_nextSpawnPeriod} ticks");
	}

	private Vector2 GetSpawnLocation()
	{
		var x = _rng.Next((int)SpawnArea.Position.X, (int)SpawnArea.End.X);
		var y = _rng.Next((int)SpawnArea.Position.Y, (int)SpawnArea.End.Y);
		return new Vector2(x, y);
	}

	private float GetSpawnPeriod()
	{
		return _rng.Next(SpawnPeriod.X, SpawnPeriod.Y);
	}
}
