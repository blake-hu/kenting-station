namespace Kenting.Interface;

// Interface for entities that should freeze when too far from players
// This is primarily an optimization to reduce the number of moving entities
// All freezable entities should start out frozen
public interface IFreeze
{
    public bool Freeze();

    public bool Unfreeze();
}