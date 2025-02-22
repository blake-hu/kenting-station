using Godot;
using KentingStation.Common.Util;
using KentingStation.Interface;

namespace KentingStation.Entity.Spawner.Strategy;

public partial class RandomSpawner<TEntity> : Node2D where TEntity : Node2D, ITrackedEntity<TEntity>
{
    private int _nextSpawnPeriod;
    private int _tickCounter;

    [Export] public PackedScene EntityScene;
    [Export] public float MaxSpawnTime;
    [Export] public float MinSpawnTime;
    [Export] public Rect2 SpawnArea;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _nextSpawnPeriod = (int)RandomScalar.GeneratePositive(MinSpawnTime, MaxSpawnTime);
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_tickCounter >= _nextSpawnPeriod)
        {
            var spawnLocation = RandomLocation.Generate(SpawnArea);
            EntityService.Singleton.Spawn<TEntity>(EntityScene, spawnLocation);
            _nextSpawnPeriod = (int)RandomScalar.GeneratePositive(MinSpawnTime, MaxSpawnTime);
            _tickCounter = 0;
        }
        else
        {
            _tickCounter++;
        }
    }
}