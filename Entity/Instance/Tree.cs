using System;
using Godot;
using Kenting.Interface;

namespace Kenting.Entity;

public partial class Tree : StaticBody2D, ITrackedEntity<Tree>
{
    private EntityContainer<Tree> _entityContainer;

    public void RegisterEntityContainer(EntityContainer<Tree> container)
    {
        _entityContainer = container;
    }

    public void Die()
    {
        if (!_entityContainer.TryRemoveEntity(this))
            throw new Exception(
                $"Internal error: Unable to remove entity {Name} from entity container {_entityContainer.GetType()} on death.");
        QueueFree();
    }
}