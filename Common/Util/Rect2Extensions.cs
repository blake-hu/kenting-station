using Godot;

namespace KentingStation.Common.Util;

public static class Rect2Extensions
{
    // Finds closest point in a Rect2 to a given point
    // If the point is already in the Rect2, returns the same point
    // Useful for enforcing WorldBoundary
    public static Vector2 ClosestPointTo(this Rect2 rect, Vector2 point)
    {
        if (point.X < rect.Position.X)
            point.X = rect.Position.X;
        if (point.X > rect.End.X)
            point.X = rect.End.X;
        if (point.Y < rect.Position.Y)
            point.Y = rect.Position.Y;
        if (point.Y > rect.End.Y)
            point.Y = rect.End.Y;
        return point;
    }
}