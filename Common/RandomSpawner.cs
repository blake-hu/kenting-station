using System;
using CozyGame.Interface;
using Godot;

namespace CozyGame.scene;

public class RandomSpawner : ISpawnerStrategy
{
    private readonly Random _rng;
    private readonly float _spawnPeriodTicks;
    private PackedScene _entityScene;
    private float _tickCounter;

    public RandomSpawner(float spawnPeriodTicks)
    {
        _spawnPeriodTicks = spawnPeriodTicks;
        _rng = new Random();
    }

    public void RegisterSceneToSpawn(PackedScene entityScene)
    {
        _entityScene = entityScene;
    }

    public void Tick(double delta)
    {
        if (_tickCounter >= _spawnPeriodTicks)
        {
            Spawn();
            _tickCounter = 0;
        }
        else
        {
            _tickCounter++;
        }
    }

    private void Spawn()
    {
        var entity = _entityScene.Instantiate<Node2D>();
        var spawnLocation = GetSpawnLocation();
        entity.Position = spawnLocation;
        MobContainer.Instance.AddChild(entity);
    }

    private Vector2 GetSpawnLocation()
    {
        var x = _rng.Next((int)WorldBoundary.X.Item1, (int)WorldBoundary.X.Item2);
        var y = _rng.Next((int)WorldBoundary.Y.Item1, (int)WorldBoundary.Y.Item2);
        return new Vector2(x, y);
    }
}