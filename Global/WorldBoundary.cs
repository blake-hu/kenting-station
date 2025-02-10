using Godot;

public partial class WorldBoundary : StaticBody2D
{
	public static Rect2 Boundary;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var xLower = GetNode<CollisionShape2D>("WestBorder").Position.X;
		var xUpper = GetNode<CollisionShape2D>("EastBorder").Position.X;
		var yLower = GetNode<CollisionShape2D>("NorthBorder").Position.Y;
		var yUpper = GetNode<CollisionShape2D>("SouthBorder").Position.Y;
		Boundary = new Rect2(xLower, yLower, xUpper, yUpper);
	}
}
