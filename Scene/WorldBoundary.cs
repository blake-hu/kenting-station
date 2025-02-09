using Godot;

public partial class WorldBoundary : StaticBody2D
{
	public static (float, float) X { get; private set; }
	public static (float, float) Y { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var xMin = GetNode<CollisionShape2D>("WestBorder").Position.X;
		var xMax = GetNode<CollisionShape2D>("EastBorder").Position.X;
		X = (xMin, xMax);

		var yMin = GetNode<CollisionShape2D>("NorthBorder").Position.Y;
		var yMax = GetNode<CollisionShape2D>("SouthBorder").Position.Y;
		Y = (yMin, yMax);

		GD.Print("X: " + X);
		GD.Print("Y: " + Y);
	}
}
