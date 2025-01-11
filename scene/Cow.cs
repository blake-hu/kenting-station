using System;
using System.Collections.Generic;
using CozyGame.Interface;
using CozyGame.scene;
using Godot;
using Vector2 = Godot.Vector2;

public partial class Cow : CharacterBody2D
{
	private AnimatedSprite2D _animatedSprite2D;
	private RandomOneAxisMover _randomOneAxisXMover;
	private RandomOneAxisMover _randomOneAxisYMover;
	private Random _rng;
	private SkittishMover _skittishMover;

	[Export] public float DiagonalWalk;
	[Export] public int MaxRunDuration = 50;
	[Export] public float MaxRunSpeed = 80f;
	[Export] public int MaxWalkDuration = 100;
	[Export] public float MaxXWalkSpeed = 20f;
	[Export] public float MaxYWalkSpeed = 10f;
	[Export] public int MinRunDuration = 20;
	[Export] public float MinRunSpeed = 30f;
	[Export] public int MinWalkDuration = 20;
	[Export] public float SkittishRadius = 100f;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_randomOneAxisXMover =
			new RandomOneAxisMover(MinWalkDuration, MaxWalkDuration, -MaxXWalkSpeed, MaxXWalkSpeed);
		_randomOneAxisYMover =
			new RandomOneAxisMover(MinWalkDuration, MaxWalkDuration, -MaxYWalkSpeed, MaxYWalkSpeed);
		var enemyGroups = new List<IUpdatingCharacterGroup> { OnlinePlayers.Singleton };
		_skittishMover =
			new SkittishMover(this, enemyGroups, SkittishRadius, MinRunDuration, MaxRunDuration, MinRunSpeed,
				MaxRunSpeed);
		_rng = new Random();
	}

	public override void _PhysicsProcess(double delta)
	{
		var move = Vector2.Zero;

		if (_skittishMover.NextMove(out var skittishMove))
		{
			GD.Print($"-- Skittish move: {skittishMove}");
			move += skittishMove;
		}

		if (_randomOneAxisXMover.NextMove(out var randomXMove))
		{
			GD.Print($"-- X move: {randomXMove}");
			move.X += randomXMove;
			// For more realistic movement, also move slightly up/down when moving horizontally
			move.Y += randomXMove * (float)_rng.NextDouble() * DiagonalWalk;
		}

		if (_randomOneAxisYMover.NextMove(out var randomYMove))
		{
			move.Y += randomYMove;
			// For more realistic movement, also move slightly left/right when moving vertically
			move.X += randomYMove * (float)_rng.NextDouble() * DiagonalWalk;
		}

		if (move != Vector2.Zero)
		{
			MoveX(move.X);
			MoveY(move.Y);
		}

		MoveAndSlide();
	}

	private void MoveX(float speed)
	{
		Velocity = speed * Vector2.Right;
		_animatedSprite2D.Scale = new Vector2(speed > 0f ? 1f : -1f, 1f);
		_animatedSprite2D.Play("right");
	}

	private void MoveY(float speed)
	{
		Velocity = new Vector2(Velocity.X, speed);
	}
}
