using System;
using System.Linq;
using CozyGame.Common;
using CozyGame.Interface;
using Godot;

namespace CozyGame.Entity;

public partial class HighDensitySpawner<TEntity> : Node2D where TEntity : Node2D, IEntity<TEntity>
{
    private readonly Random _rng = new();
    private ChunkedEntityCounter<TEntity> _chunkedCounter;

    private float _nextSpawnPeriod;
    private float _tickCounter;

    [Export] public PackedScene EntityScene;
    [Export] public float MaxSpawnTime;
    [Export] public float MinSpawnTime;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ValidateSpawnPeriod();
        _nextSpawnPeriod = GetSpawnPeriod();
        var entityContainer = EntityService.Singleton.GetContainer<TEntity>();
        _chunkedCounter = entityContainer.ChunkedCounter;
    }

    private void ValidateSpawnPeriod()
    {
        if (MinSpawnTime > MaxSpawnTime)
            throw new ArgumentException(
                $"First value ({MinSpawnTime}) of SpawnPeriodRange must be smaller than MaxSpawnTime value ({MaxSpawnTime}).");
        if (MinSpawnTime < 0.0f)
            throw new ArgumentException($"First value ({MinSpawnTime}) of SpawnPeriodRange must be a positive value.");
        if (MinSpawnTime < 5.0f)
            throw new Exception($"Min spawn time for {typeof(TEntity)} is unusually fast. Are you sure?");
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_tickCounter >= _nextSpawnPeriod && GetSpawnLocation(out var location))
        {
            EntityService.Singleton.Spawn<TEntity>(EntityScene, location);
            _nextSpawnPeriod = GetSpawnPeriod();
            _tickCounter = 0;
        }
        else
        {
            _tickCounter++;
        }
    }

    private bool GetSpawnLocation(out Vector2 location)
    {
        _chunkedCounter.UpdateCounts();
        var chunkedCounts = _chunkedCounter.CachedCounts;
        if (chunkedCounts.Count == 0)
        {
            location = default;
            return false;
        }

        var orderedCounts = chunkedCounts.OrderBy(kvp => kvp.Value).Reverse();
        var densestChunk = orderedCounts.First().Key;
        var chunkSize = _chunkedCounter.ChunkSize;
        var spawnArea = new Chunk(densestChunk, chunkSize).Boundary();
        location = RandomLocation.Generate(spawnArea);
        return true;
    }

    private float GetSpawnPeriod()
    {
        return _rng.Next((int)MinSpawnTime, (int)MaxSpawnTime);
    }
}