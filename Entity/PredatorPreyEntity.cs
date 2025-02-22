using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using Godot;
using Kenting.Common;
using KentingStation.Common;
using KentingStation.Common.Util;
using KentingStation.Exception;
using KentingStation.Interface;

namespace KentingStation.Entity;

public partial class PredatorPreyEntity<TEntity> : CharacterBody2D, IPredatorPreyEntity
    where TEntity : CharacterBody2D
{
    private AnimatedSprite2D _animatedSprite2D;
    private EntityContainer<TEntity> _entityContainer;
    private bool _frozen;
    private PredatorPreyMover _predatorPreyMover;
    private RandomOneAxisMover _randomOneAxisXMover;
    private RandomOneAxisMover _randomOneAxisYMover;

    // Default values simulate movement of cow, can be overwritten by users in Godot
    [Export] public float DiagonalWalk;
    [Export] public int MaxRunDuration = 50;
    [Export] public float MaxRunSpeed = 80f;
    [Export] public int MaxWalkDuration = 100;
    [Export] public float MaxXWalkSpeed = 20f;
    [Export] public float MaxYWalkSpeed = 10f;
    [Export] public int MinRunDuration = 20;
    [Export] public float MinRunSpeed = 30f;
    [Export] public int MinWalkDuration = 20;

    // By default, the entity has no prey or predators
    // Override these properties to add prey or predators
    protected virtual HashSet<Type> Prey { get; } = [];
    protected virtual HashSet<Type> Predators { get; } = [];

    public FrozenSet<Type> PreyTypes()
    {
        return Prey.ToFrozenSet();
    }

    public Type EntityType()
    {
        return typeof(TEntity);
    }

    public FrozenSet<Type> PredatorTypes()
    {
        return Predators.ToFrozenSet();
    }

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

    public void RegisterEntityContainer(EntityContainer<TEntity> container)
    {
        _entityContainer = container;
    }

    protected virtual void DieCustomLogic()
    {
    }

    public void Die()
    {
        DieCustomLogic();
        if (this is not TEntity derivedEntity)
            throw new KsInvalidCastException(nameof(Die), nameof(PredatorPreyEntity<TEntity>), nameof(TEntity),
                "Likely because entity inheritance hierarchy was not set up correctly.");
        if (!_entityContainer.TryRemoveEntity(derivedEntity))
            throw new System.Exception(
                $"Internal error: Unable to remove entity {Name} from entity container {_entityContainer.GetType()} on death.");
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
        _predatorPreyMover = GetNode<PredatorPreyMover>("PredatorPreyMover");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_frozen) return;

        var move = Vector2.Zero;

        if (_predatorPreyMover.NextMove(out var skittishMove))
            move += skittishMove;

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
        var absX = Math.Abs(scale.X);
        if (speed > 0f)
            scale.X = absX;
        else
            scale.X = -absX; // Flip sprite horizontally
        _animatedSprite2D.Scale = scale;

        _animatedSprite2D.Play("right");
    }

    private void MoveY(float speed)
    {
        Velocity = new Vector2(Velocity.X, speed);
    }
}