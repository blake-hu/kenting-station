using System.Linq;
using Godot;
using Kenting.Common;
using Kenting.Interface;

namespace Kenting.Entity.Spawner.Strategy;

public partial class DensitySpawner<TEntity> : Node2D where TEntity : Node2D, IEntity<TEntity>
{
    private ChunkedEntityCounter<TEntity> _chunkedCounter;
    private int _nextSpawnPeriod;
    private int _tickCounter;

    [Export] public PackedScene EntityScene;
    [Export] public float MaxSpawnTime;
    [Export] public float MinSpawnTime;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _nextSpawnPeriod = (int)RandomScalar.GeneratePositive(MinSpawnTime, MaxSpawnTime);
        var entityContainer = EntityService.Singleton.GetContainer<TEntity>();
        _chunkedCounter = entityContainer.ChunkedCounter;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_tickCounter >= _nextSpawnPeriod && GetSpawnLocation(out var location))
        {
            EntityService.Singleton.Spawn<TEntity>(EntityScene, location);
            _nextSpawnPeriod = (int)RandomScalar.GeneratePositive(MinSpawnTime, MaxSpawnTime);
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
}