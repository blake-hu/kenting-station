using System;
using System.Collections.Generic;
using Godot;
using Kenting.Common;
using Kenting.Interface;
using KentingStation.Item;
using Vector2 = Godot.Vector2;

namespace Kenting.Entity;

public partial class Cow : CharacterBody2D, ITrackedEntity<Cow>, IFreeze
{
    private AnimatedSprite2D _animatedSprite2D;
    private EntityContainer<Cow> _entityContainer;
    private bool _frozen;
    private RandomOneAxisMover _randomOneAxisXMover;
    private RandomOneAxisMover _randomOneAxisYMover;
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

    public bool Freeze()
    {
        _frozen = true;
        _animatedSprite2D?.Stop();
        return true;
    }

    public bool Unfreeze()
    {
        _frozen = false;
        return true;
    }

    public void RegisterEntityContainer(EntityContainer<Cow> container)
    {
        _entityContainer = container;
    }

    public void Die()
    {
        if (!_entityContainer.TryRemoveEntity(this))
            throw new Exception(
                $"Internal error: Unable to remove entity {Name} from entity container {_entityContainer.GetType()} on death.");
        var beef = ItemProvider.Singleton.Get<Beef>();
        ItemDropService.Singleton.Spawn(beef, 1, Position);
        GD.Print($"{Name} was killed");
        QueueFree();
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _randomOneAxisXMover =
            new RandomOneAxisMover(MinWalkDuration, MaxWalkDuration, -MaxXWalkSpeed, MaxXWalkSpeed);
        _randomOneAxisYMover =
            new RandomOneAxisMover(MinWalkDuration, MaxWalkDuration, -MaxYWalkSpeed, MaxYWalkSpeed);
        var enemyGroups = new List<IUpdatingGroup<CharacterBody2D>> { OnlinePlayers.Singleton };
        _skittishMover =
            new SkittishMover(this, enemyGroups, SkittishRadius, MinRunDuration, MaxRunDuration, MinRunSpeed,
                MaxRunSpeed);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_frozen) return;

        var move = Vector2.Zero;

        if (_skittishMover.NextMove(out var skittishMove)) move += skittishMove;

        if (_randomOneAxisXMover.NextMove(out var randomXMove))
        {
            move.X += randomXMove;
            // For more realistic movement, also move slightly up/down when moving horizontally
            move.Y += randomXMove * RandomScalar.Generate(-DiagonalWalk, DiagonalWalk);
        }

        if (_randomOneAxisYMover.NextMove(out var randomYMove))
        {
            move.Y += randomYMove;
            // For more realistic movement, also move slightly left/right when moving vertically
            move.X += randomYMove * RandomScalar.Generate(-DiagonalWalk, DiagonalWalk);
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
        var scale = _animatedSprite2D.Scale;
        scale.X *= -1; // Invert sprite horizontally
        _animatedSprite2D.Scale = scale;
        _animatedSprite2D.Play("right");
    }

    private void MoveY(float speed)
    {
        Velocity = new Vector2(Velocity.X, speed);
    }
}