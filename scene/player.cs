using Godot;
using System;

public partial class player : CharacterBody2D
{
	public const float Speed = 100.0f;

	public override void _PhysicsProcess(double delta)
	{
		Velocity = Input.GetVector("left", "right", "up", "down") * Speed;
		MoveAndSlide();
	}
}
