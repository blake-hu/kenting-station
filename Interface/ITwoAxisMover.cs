using Godot;

namespace CozyGame.Interface;

public interface ITwoAxisMover
{
    public bool NextMove(out Vector2 moveValues);
}