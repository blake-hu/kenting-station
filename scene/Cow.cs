using System;
using CozyGame.scene;
using Godot;
using Vector2 = Godot.Vector2;

public partial class Cow : CharacterBody2D
{
    private AnimatedSprite2D _animatedSprite2D;
    private RandomOneAxisMoverMover _randomOneAxisXMoverMover;
    private RandomOneAxisMoverMover _randomOneAxisYMoverMover;
    private Random _rng;
    private SkittishMover _skittishMover;

    [Export] public int MaxMoveDuration = 100;
    [Export] public int MaxRunDuration = 100;
    [Export] public float MaxRunSpeed = 80f;
    [Export] public float MaxXSpeed = 100f;
    [Export] public float MaxYSpeed = 50f;
    [Export] public int MinMoveDuration = 20;
    [Export] public int MinRunDuration = 20;
    [Export] public float MinRunSpeed = 30f;
    [Export] public float SkittishRadius = 100f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _randomOneAxisXMoverMover =
            new RandomOneAxisMoverMover(MinMoveDuration, MaxMoveDuration, -MaxXSpeed, MaxXSpeed);
        _randomOneAxisYMoverMover =
            new RandomOneAxisMoverMover(MinMoveDuration, MaxMoveDuration, -MaxYSpeed, MaxYSpeed);
        _skittishMover =
            new SkittishMover(this, SkittishRadius, MinRunDuration, MaxRunDuration, MinRunSpeed, MaxRunSpeed);
        _rng = new Random();
    }

    public override void _PhysicsProcess(double delta)
    {
        var move = Vector2.Zero;

        if (_skittishMover.NextMove(out var skittishMove))
            move += skittishMove;

        if (_randomOneAxisXMoverMover.NextMove(out var randomXMove))
        {
            move.X += randomXMove;
            move.Y += randomXMove * (float)_rng.NextDouble() * 0.2f;
        }

        if (_randomOneAxisYMoverMover.NextMove(out var randomYMove))
        {
            move.Y += randomYMove;
            move.X += randomXMove * (float)_rng.NextDouble() * 0.2f;
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