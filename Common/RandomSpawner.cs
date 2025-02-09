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
        var boundaryLower = WorldBoundary.Boundary.Lower;
        var boundaryUpper = WorldBoundary.Boundary.Upper;
        var x = _rng.Next((int)boundaryLower.X, (int)boundaryUpper.X);
        var y = _rng.Next((int)boundaryLower.Y, (int)boundaryUpper.Y);
        return new Vector2(x, y);
    }
}