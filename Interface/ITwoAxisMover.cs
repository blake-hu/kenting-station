using Godot;

namespace KentingStation.Interface;

public interface ITwoAxisMover
{
    public bool NextMove(out Vector2 moveValues);
}