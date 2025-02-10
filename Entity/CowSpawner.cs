using Godot;

namespace CozyGame.Entity;

public partial class CowSpawner : RandomSpawner<Cow>
{
    [Export]
    public PackedScene EntityScene
    {
        get => _entityScene;
        set => _entityScene = value;
    }

    [Export]
    public float MinSpawnTime
    {
        get => _minSpawnTime;
        set => _minSpawnTime = value;
    }

    [Export]
    public float MaxSpawnTime
    {
        get => _maxSpawnTime;
        set => _maxSpawnTime = value;
    }

    [Export]
    public Rect2 SpawnArea
    {
        get => _spawnArea;
        set => _spawnArea = value;
    }
}