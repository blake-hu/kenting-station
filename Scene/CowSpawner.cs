using CozyGame.Interface;
using CozyGame.scene;
using Godot;

public partial class CowSpawner : Node2D
{
    private ISpawnerStrategy _spawnerStrategy;
    [Export] public PackedScene CowScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _spawnerStrategy = new RandomSpawner(30);
        _spawnerStrategy.RegisterSceneToSpawn(CowScene);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        _spawnerStrategy.Tick(delta);
    }
}