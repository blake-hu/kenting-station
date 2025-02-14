using Godot;

namespace Kenting.Common;

public static class Rect2Ex
{
    // Finds closest point in a Rect2 to a given point
    // If the point is already in the Rect2, returns the same point
    // Useful for enforcing WorldBoundary
    public static Vector2 ClosestPointWithinRect(Rect2 rect, Vector2 point)
    {
        return Vector2.Zero; // Should fail
    }
}