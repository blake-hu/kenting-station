using Godot;
using Kenting.Common;

namespace Kenting.Tests;

public class Rect2ExTests
{
    private static readonly Rect2 Rect = new(new Vector2(-10, -10), new Vector2(20, 20));

    [Test]
    public void PointInRect(
        [Random(-10, 10, 10)] double x,
        [Random(-10, 10, 10)] double y)
    {
        var point = new Vector2((float)x, (float)y);
        Assert.That(point, Is.EqualTo(Rect2Ex.ClosestPointWithinRect(Rect, point)));
    }

    [Test]
    public void PointInRectEdge(
        [Values(-10, 10)] double x,
        [Values(-10, 10)] double y)
    {
        var point = new Vector2((float)x, (float)y);
        Assert.That(point, Is.EqualTo(Rect2Ex.ClosestPointWithinRect(Rect, point)));
    }

    [Test]
    public void PointOutsideRectXLeft(
        [Random(-100, -10.1, 10)] double x,
        [Random(-10, 10, 10)] double y)
    {
        var point = new Vector2((float)x, (float)y);
        var closest = new Vector2(-10, (float)y);
        Assert.That(closest, Is.EqualTo(Rect2Ex.ClosestPointWithinRect(Rect, point)));
    }

    [Test]
    public void PointOutsideRectXRight(
        [Random(10.1, 100, 10)] double x,
        [Random(-10, 10, 10)] double y)
    {
        var point = new Vector2((float)x, (float)y);
        var closest = new Vector2(10, (float)y);
        Assert.That(closest, Is.EqualTo(Rect2Ex.ClosestPointWithinRect(Rect, point)));
    }

    [Test]
    public void PointOutsideRectYUpper( // Upper because upper half is negative in Godot
        [Random(-10, 10, 10)] double x,
        [Random(-100, -10.1, 10)] double y)
    {
        var point = new Vector2((float)x, (float)y);
        var closest = new Vector2((float)x, -10);
        Assert.That(closest, Is.EqualTo(Rect2Ex.ClosestPointWithinRect(Rect, point)));
    }

    [Test]
    public void PointOutsideRectYLower( // Lower because lower half is positive in Godot
        [Random(-10, 10, 10)] double x,
        [Random(10.1, 100, 10)] double y)
    {
        var point = new Vector2((float)x, (float)y);
        var closest = new Vector2((float)x, 10);
        Assert.That(closest, Is.EqualTo(Rect2Ex.ClosestPointWithinRect(Rect, point)));
    }

    [Test]
    public void PointOutsideRectUpperLeft(
        [Random(-100, -10.1, 10)] double x,
        [Random(-100, -10.1, 10)] double y)
    {
        var point = new Vector2((float)x, (float)y);
        var closest = new Vector2(-10, -10);
        Assert.That(closest, Is.EqualTo(Rect2Ex.ClosestPointWithinRect(Rect, point)));
    }

    [Test]
    public void PointOutsideRectUpperRight(
        [Random(10.1, 100, 10)] double x,
        [Random(-100, -10.1, 10)] double y)
    {
        var point = new Vector2((float)x, (float)y);
        var closest = new Vector2(10, -10);
        Assert.That(closest, Is.EqualTo(Rect2Ex.ClosestPointWithinRect(Rect, point)));
    }

    [Test]
    public void PointOutsideRectLowerLeft(
        [Random(-100, -10.1, 10)] double x,
        [Random(10.1, 100, 10)] double y)
    {
        var point = new Vector2((float)x, (float)y);
        var closest = new Vector2(-10, 10);
        Assert.That(closest, Is.EqualTo(Rect2Ex.ClosestPointWithinRect(Rect, point)));
    }

    [Test]
    public void PointOutsideRectLowerRight(
        [Random(10.1, 100, 10)] double x,
        [Random(10.1, 100, 10)] double y)
    {
        var point = new Vector2((float)x, (float)y);
        var closest = new Vector2(10, 10);
        Assert.That(closest, Is.EqualTo(Rect2Ex.ClosestPointWithinRect(Rect, point)));
    }
}