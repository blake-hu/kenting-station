using System;
using CozyGame.Interface;
using Godot;

namespace CozyGame.Entity;

public partial class RandomSpawner<TEntity> : Node2D where TEntity : Node2D, IEntity<TEntity>
{
    private readonly Random _rng = new();
    protected PackedScene _entityScene;
    protected float _maxSpawnTime;
    protected float _minSpawnTime;

    private float _nextSpawnPeriod;
    protected Rect2 _spawnArea;
    private float _tickCounter;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ValidateSpawnPeriod();
        _nextSpawnPeriod = GetSpawnPeriod();
    }

    private void ValidateSpawnPeriod()
    {
        if (_minSpawnTime > _maxSpawnTime)
            throw new ArgumentException(
                $"First value ({_minSpawnTime}) of SpawnPeriodRange must be smaller than _maxSpawnTime value ({_maxSpawnTime}).");
        if (_minSpawnTime < 0.0f)
            throw new ArgumentException($"First value ({_minSpawnTime}) of SpawnPeriodRange must be a positive value.");
        if (_minSpawnTime < 5.0f)
            throw new Exception($"Min spawn time for {typeof(TEntity)} is unusually fast. Are you sure?");
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_tickCounter >= _nextSpawnPeriod)
        {
            EntityService.Singleton.Spawn<TEntity>(_entityScene, GetSpawnLocation());
            _nextSpawnPeriod = GetSpawnPeriod();
            _tickCounter = 0;
        }
        else
        {
            _tickCounter++;
        }
    }

    private Vector2 GetSpawnLocation()
    {
        var x = _rng.Next((int)_spawnArea.Position.X, (int)_spawnArea.End.X);
        var y = _rng.Next((int)_spawnArea.Position.Y, (int)_spawnArea.End.Y);
        return new Vector2(x, y);
    }

    private float GetSpawnPeriod()
    {
        return _rng.Next((int)_minSpawnTime, (int)_maxSpawnTime);
    }
}