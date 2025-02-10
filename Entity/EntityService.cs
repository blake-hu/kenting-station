using System;
using System.Collections.Generic;
using System.Text;
using CozyGame.Common;
using Godot;

namespace CozyGame.Entity;

public partial class EntityService : Node2D
{
    private readonly Dictionary<Type, IEntityContainer<Node2D>> _entityContainers = new()
    {
        { typeof(Cow), new EntityContainer<Cow>(new ChunkedEntityCounter<Cow>(128, 200)) },
        { typeof(Tree), new EntityContainer<Tree>(new ChunkedEntityCounter<Tree>(256, 400)) }
    };

    [Export] public int CounterTimeToLive;

    public static EntityService Singleton { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Singleton = this;
        if (CounterTimeToLive < 0)
            throw new Exception(
                $"TimeToLive for ChunkedEntityCounter is {CounterTimeToLive}, but must be non-negative.");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        foreach (var container in _entityContainers.Values)
            container.Tick();
    }

    public EntityContainer<TEntity> GetContainer<TEntity>() where TEntity : Node2D
    {
        if (_entityContainers.TryGetValue(typeof(TEntity), out var container))
        {
            if (container is EntityContainer<TEntity> concreteContainer)
                return concreteContainer;
            throw new Exception(
                $"Internal error: Could not cast IEntityContainer<Node2D>> {container} to EntityContainer<{typeof(TEntity)}>." +
                $"This is likely because EntityService is not correctly configured.");
        }

        throw new Exception($"Key error in EntityService: Could not find EntityContainer for {typeof(TEntity)}");
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("---- EntityService State ----");
        foreach (var entityContainer in _entityContainers.Values) sb.AppendLine(entityContainer.ToString());
        return sb.ToString();
    }
}