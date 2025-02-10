using System;
using CozyGame.Common;
using CozyGame.Interface;
using Godot;

namespace CozyGame.Entity.Spawner.Strategy;

public partial class RandomSpawner<TEntity> : Node2D where TEntity : Node2D, IEntity<TEntity>
{
    private readonly Random _rng = new();
    private float _nextSpawnPeriod;
    private float _tickCounter;

    [Export] public PackedScene EntityScene;
    [Export] public float MaxSpawnTime;
    [Export] public float MinSpawnTime;
    [Export] public Rect2 SpawnArea;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ValidateSpawnPeriod();
        _nextSpawnPeriod = GetSpawnPeriod();
    }

    private void ValidateSpawnPeriod()
    {
        if (MinSpawnTime > MaxSpawnTime)
            throw new ArgumentException(
                $"First value ({MinSpawnTime}) of SpawnPeriodRange must be smaller than _maxSpawnTime value ({MaxSpawnTime}).");
        if (MinSpawnTime < 0.0f)
            throw new ArgumentException($"First value ({MinSpawnTime}) of SpawnPeriodRange must be a positive value.");
        if (MinSpawnTime < 5.0f)
            throw new Exception($"Min spawn time for {typeof(TEntity)} is unusually fast. Are you sure?");
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_tickCounter >= _nextSpawnPeriod)
        {
            var spawnLocation = RandomLocation.Generate(SpawnArea);
            EntityService.Singleton.Spawn<TEntity>(EntityScene, spawnLocation);
            _nextSpawnPeriod = GetSpawnPeriod();
            _tickCounter = 0;
        }
        else
        {
            _tickCounter++;
        }
    }

    private float GetSpawnPeriod()
    {
        return _rng.Next((int)MinSpawnTime, (int)MaxSpawnTime);
    }
}