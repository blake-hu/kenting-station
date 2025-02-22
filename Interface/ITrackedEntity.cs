using Godot;
using KentingStation.Entity;

namespace KentingStation.Interface;

// Represents in-game characters which are tracked with an entity container
public interface ITrackedEntity<TEntity> : IKillableEntity where TEntity : Node2D
{
    public void RegisterEntityContainer(EntityContainer<TEntity> container);
}