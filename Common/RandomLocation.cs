using System;
using Godot;

namespace CozyGame.Common;

public static class RandomLocation
{
    private static readonly Random Rng = new();

    public static Vector2 Generate(Rect2 area)
    {
        var x = Rng.Next((int)area.Position.X, (int)area.End.X);
        var y = Rng.Next((int)area.Position.Y, (int)area.End.Y);
        return new Vector2(x, y);
    }
}