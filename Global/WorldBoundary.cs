using Godot;

public partial class WorldBoundary : StaticBody2D
{
	public static WorldBoundary Singleton;

	[Export] public Rect2 Boundary;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Singleton = this;

		var west = GetNode<CollisionShape2D>("WestBorder");
		var east = GetNode<CollisionShape2D>("EastBorder");
		var north = GetNode<CollisionShape2D>("NorthBorder");
		var south = GetNode<CollisionShape2D>("SouthBorder");

		west.Position = new Vector2(Boundary.Position.X, 0);
		east.Position = new Vector2(Boundary.End.X, 0);
		north.Position = new Vector2(0, Boundary.Position.Y);
		south.Position = new Vector2(0, Boundary.End.Y);
	}
}
