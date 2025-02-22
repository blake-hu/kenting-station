using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;
using KentingStation.Interface;

namespace KentingStation.Entity;

public class EntityContainer<TEntity> : IEntityContainer<TEntity> where TEntity : Node2D
{
    private readonly Dictionary<EntityId, TEntity> _activeEntities = new();

    public EntityContainer()
    {
    }

    public EntityContainer(ChunkedEntityCounter<TEntity> chunkedCounter)
    {
        ChunkedCounter = chunkedCounter;
        chunkedCounter.RegisterEntityDict(_activeEntities);
    }

    public ChunkedEntityCounter<TEntity> ChunkedCounter { get; }

    public void Tick()
    {
        ChunkedCounter?.Tick();
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

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"EntityContainer<{typeof(TEntity)}>: {_activeEntities.Count} entities");
        foreach (var (entityId, i) in _activeEntities.Keys.Select((value, i) => (value, i)))
            sb.AppendLine($"{i}: {entityId}");
        return sb.ToString();
    }
}