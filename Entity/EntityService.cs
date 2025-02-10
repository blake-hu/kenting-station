using System;
using System.Collections.Generic;
using System.Text;
using CozyGame.scene;
using Godot;

public partial class EntityService : Node2D
{
    private readonly Dictionary<Type, IEntityContainer<Node2D>> _entityContainers = new()
    {
        { typeof(Cow), new EntityContainer<Cow>() }
    };

    public static EntityService Instance { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
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