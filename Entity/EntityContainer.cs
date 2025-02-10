using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CozyGame.Common;
using Godot;

namespace CozyGame.Entity;

// Interface added to allow TEntity type parameter to be covariant so that EntityService can store EntityContainers
// parameterized by different entity types derived from Node2D
public interface IEntityContainer<out TEntity> where TEntity : Node2D
{
}

public class EntityContainer<TEntity> : IEntityContainer<TEntity> where TEntity : Node2D
{
    private readonly Dictionary<EntityId, TEntity> _activeEntities = new();
    private readonly ChunkedEntityCounter<TEntity> _chunkedCounter;

    public EntityContainer()
    {
    }

    public EntityContainer(ChunkedEntityCounter<TEntity> chunkedCounter)
    {
        _chunkedCounter = chunkedCounter;
        chunkedCounter.RegisterEntityDict(_activeEntities);
    }

    public bool TryAddEntity(TEntity entity)
    {
        return _activeEntities.TryAdd(entity.Name, entity);
    }

    public bool TryRemoveEntity(TEntity entity)
    {
        var id = (EntityId)entity.Name;
        return _activeEntities.Remove(id);
    }

    private void CheckChunkedCounterAvailable()
    {
        if (_chunkedCounter == null)
            throw new Exception($"No ChunkedEntityCounter available for {typeof(EntityContainer<TEntity>)}");
    }

    public uint[,] GetChunkedCountsCached()
    {
        CheckChunkedCounterAvailable();
        return _chunkedCounter.CachedCounts;
    }

    public void RefreshChunkedCounts()
    {
        CheckChunkedCounterAvailable();
        _chunkedCounter.RefreshCounts();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"EntityContainer<{typeof(TEntity)}>: {_activeEntities.Count} entities");
        foreach (var (entityId, i) in _activeEntities.Keys.Select((value, i) => (value, i)))
            sb.AppendLine($"{i}: {entityId}");
        return sb.ToString();
    }
}