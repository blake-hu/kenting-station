using Godot;

namespace Kenting.Interface;

public interface ITwoAxisMover
{
    public bool NextMove(out Vector2 moveValues);
}