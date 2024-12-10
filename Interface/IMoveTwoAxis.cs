using Godot;

namespace CozyGame.Interface;

public interface IMoveTwoAxis
{
    public bool NextMove(out Vector2 moveValues);
}