using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using KentingStation.Common.Util;
using KentingStation.Entity.Instance;
using KentingStation.Exception;
using KentingStation.Interface;
using Tiger = KentingStation.Entity.Instance.Tiger;
using Tree = KentingStation.Entity.Instance.Tree;
using WorldBoundary = KentingStation.Common.WorldBoundary;

namespace KentingStation.Entity;

public partial class EntityService : Node2D
{
    private readonly Dictionary<Type, IEntityContainer<Node2D>> _entityContainers = new()
    {
        { typeof(Cow), new EntityContainer<Cow>(new ChunkedEntityCounter<Cow>(128, 200)) },
        { typeof(Tiger), new EntityContainer<Tiger>(new ChunkedEntityCounter<Tiger>(128, 200)) },
        { typeof(Tree), new EntityContainer<Tree>(new ChunkedEntityCounter<Tree>(256, 400)) },
        { typeof(Grass), new EntityContainer<Grass>(new ChunkedEntityCounter<Grass>(64, 100)) }
    };

    private int _printInterval = 60;
    private int _timestamp;

    [Export] public int CounterTimeToLive;
    [Export] public bool PrintEntityCounts;

    private EntityService() // prevent public instantiation
    {
    }

    public static EntityService Singleton { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Singleton = this;
        if (CounterTimeToLive < 0)
            throw new System.Exception(
                $"TimeToLive for ChunkedEntityCounter is {CounterTimeToLive}, but must be non-negative.");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        foreach (var container in _entityContainers.Values)
            container.Tick();

        if (PrintEntityCounts) PrintCounts();
    }

    private void PrintCounts()
    {
        _timestamp += 1;
        if (_timestamp % _printInterval == 0)
        {
            foreach (var (type, container) in _entityContainers)
                GD.Print($"{type}: {container.EntityCount()}");
            GD.Print();
        }
    }

    public EntityContainer<TEntity> GetContainer<TEntity>() where TEntity : Node2D
    {
        if (_entityContainers.TryGetValue(typeof(TEntity), out var container))
        {
            if (container is EntityContainer<TEntity> concreteContainer)
                return concreteContainer;
            throw new KsInvalidCastException(nameof(EntityService), nameof(container),
                typeof(EntityContainer<TEntity>).ToString(),
                $"This is likely because the dictionary in {nameof(EntityService)} was not properly configured");
        }

        throw new KsKeyNotFoundException(nameof(EntityService), typeof(TEntity).ToString(), _entityContainers);
    }

    public void Spawn<TEntity>(PackedScene entityScene, Vector2 spawnLocation)
        where TEntity : Node2D, ITrackedEntity<TEntity>
    {
        var entity = entityScene.Instantiate<TEntity>();
        entity.Name = new EntityId(entity.Name);

        var boundedSpawnLocation = WorldBoundary.Singleton.Boundary.ClosestPointTo(spawnLocation);
        entity.Position = boundedSpawnLocation;

        // Because entities spawned outside UnfreezeArea do not trigger BodyExited event,
        // all freezable entities must start out frozen 
        if (entity is IFreeze freezableEntity) freezableEntity.Freeze();

        Singleton.AddChild(entity);
        var container = Singleton.GetContainer<TEntity>();
        if (!container.TryAddEntity(entity))
            throw new System.Exception($"Internal error: Unable to add entity {entity.Name} to container");
        entity.RegisterEntityContainer(container);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("---- EntityService State ----");
        foreach (var entityContainer in _entityContainers.Values) sb.AppendLine(entityContainer.ToString());
        return sb.ToString();
    }
}