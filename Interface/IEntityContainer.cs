using Godot;

namespace KentingStation.Interface;

// Interface added to allow TEntity type parameter to be covariant so that EntityService can store EntityContainers
// parameterized by different entity types derived from Node2D
public interface IEntityContainer<out TEntity> where TEntity : Node2D
{
    public void Tick();

    public int EntityCount();
}