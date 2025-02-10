using System;
using CozyGame.Interface;
using CozyGame.scene;
using Godot;

namespace CozyGame.Entity;

public partial class RandomSpawner<TEntity> : Node2D where TEntity : Node2D, IEntity<TEntity>
{
    private readonly Random _rng = new();
    protected PackedScene _entityScene;

    private float _nextSpawnPeriod;
    protected Rect2 _spawnArea;
    protected Vector2I _spawnPeriod;
    private float _tickCounter;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ValidateSpawnPeriod();
        _nextSpawnPeriod = GetSpawnPeriod();
    }

    private void ValidateSpawnPeriod()
    {
        var first = _spawnPeriod.X;
        var second = _spawnPeriod.Y;
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
        var entity = _entityScene.Instantiate<TEntity>();
        var entityType = entity.GetType();
        var spawnLocation = GetSpawnLocation();
        entity.Position = spawnLocation;
        entity.Name = new EntityId(entity.Name);
        EntityService.Instance.AddChild(entity);
        var container = EntityService.Instance.GetContainer<TEntity>();
        if (!container.TryAddEntity(entity))
            throw new Exception($"Internal error: Unable to add entity {entity.Name} to container");
        entity.RegisterEntityContainer(container);
        GD.Print($"Spawned {entity.Name} at location {spawnLocation} after {_nextSpawnPeriod} ticks");
        GD.Print(EntityService.Instance.ToString());
    }

    private Vector2 GetSpawnLocation()
    {
        var x = _rng.Next((int)_spawnArea.Position.X, (int)_spawnArea.End.X);
        var y = _rng.Next((int)_spawnArea.Position.Y, (int)_spawnArea.End.Y);
        return new Vector2(x, y);
    }

    private float GetSpawnPeriod()
    {
        return _rng.Next(_spawnPeriod.X, _spawnPeriod.Y);
    }
}