using Godot;

namespace CozyGame.Interface;

public interface ISpawnerStrategy
{
    void RegisterSceneToSpawn(PackedScene entityScene);

    void Tick(double delta);
}