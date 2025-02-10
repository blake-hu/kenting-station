using CozyGame.scene;
using Godot;

namespace CozyGame.Interface;

public interface IEntity<TEntity> : IEntity where TEntity : Node2D
{
    public void RegisterEntityContainer(EntityContainer<TEntity> container);
}

public interface IEntity
{
    public void Die();
}