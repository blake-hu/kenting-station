using Godot;
using KentingStation.Interface;

namespace KentingStation.Entity.Instance;

public partial class Tree : StaticBody2D, ITrackedEntity<Tree>
{
    private bool _died;
    private EntityContainer<Tree> _entityContainer;

    public void RegisterEntityContainer(EntityContainer<Tree> container)
    {
        _entityContainer = container;
    }

    public void QueueDie()
    {
        _died = true;
    }

    private void Die()
    {
        if (!_entityContainer.TryRemoveEntity(this))
            throw new System.Exception(
                $"Internal error: Unable to remove entity {Name} from entity container {_entityContainer.GetType()} on death.");
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_died) Die();
    }
}