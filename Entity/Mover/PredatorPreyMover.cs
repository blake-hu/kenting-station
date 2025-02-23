using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using Godot;
using KentingStation.Common.Util;
using KentingStation.Interface;

namespace KentingStation.Entity.Mover;

public partial class PredatorPreyMover : Area2D
{
    private readonly List<CharacterBody2D> _predators = [];
    private readonly List<CharacterBody2D> _prey = [];
    private readonly RandomDelay _randomDelay;
    private CharacterBody2D _parentEntity;
    private FrozenSet<Type> _predatorTypes;
    private FrozenSet<Type> _preyTypes;
    [Export] public float DetectionRadius;

    [Export] public int MaxUpdateDelay;
    [Export] public int MinUpdateDelay;
    [Export] public int SpeedMultiplier;

    public PredatorPreyMover()
    {
        _randomDelay =
            new RandomDelay(MinUpdateDelay, MaxUpdateDelay);
    }

    public bool NextMove(out Vector2 moveValues)
    {
        if (!_randomDelay.Done())
        {
            moveValues = Vector2.Zero;
            return false;
        }

        moveValues = ComputeMoveVector();
        return true;
    }

    private Vector2 ComputeMoveVector()
    {
        var runToPrey = ComputeMoveVectorFor(_prey);
        var runFromPredator = ComputeMoveVectorFor(_predators);
        var netVector = runToPrey - runFromPredator;
        return netVector * SpeedMultiplier;
    }

    private Vector2 ComputeMoveVectorFor(List<CharacterBody2D> entities)
    {
        var netMove = Vector2.Zero;
        var parentPos = _parentEntity.GlobalPosition;
        foreach (var entity in entities)
        {
            var parentToEntity = entity.GlobalPosition - parentPos;
            netMove += ScaleMovementVector(parentToEntity);
        }

        return netMove;
    }

    private Vector2 ScaleMovementVector(Vector2 unscaled)
    {
        // Normalize to unit vector first
        var normalizedVector = unscaled.Normalized();

        // Amount of movement should be inversely proportional to ratio of distance to DetectionRadius
        // Motivation: the closer a predator/prey is, the faster you run
        var scaledVector = normalizedVector * DetectionRadius / unscaled.Length();
        return scaledVector;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var parentNode = GetParent();
        if (parentNode is not IPredatorPreyEntity predPreyEntity)
            throw new System.Exception($"Parent of {nameof(PredatorPreyMover)} must be {nameof(IPredatorPreyEntity)}");
        _predatorTypes = predPreyEntity.PredatorTypes();
        _preyTypes = predPreyEntity.PreyTypes();

        if (parentNode is not CharacterBody2D charBody)
            throw new System.Exception($"Parent of {nameof(PredatorPreyMover)} must be {nameof(CharacterBody2D)}");
        _parentEntity = charBody;

        var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        if (collisionShape.Shape is not CircleShape2D circle)
            throw new System.Exception(
                $"{nameof(CollisionShape2D)} must be a {nameof(CircleShape2D)} in {nameof(PredatorPreyMover)}");
        circle.Radius = DetectionRadius;

        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is not CharacterBody2D entity)
            return;
        if (entity is not IPredatorPreyEntity predPreyEntity)
            return;

        var entityType = predPreyEntity.EntityType();
        if (_predatorTypes.Contains(entityType))
            _predators.Remove(entity);
        if (_preyTypes.Contains(entityType))
            _prey.Remove(entity);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not CharacterBody2D entity)
            return;
        if (entity is not IPredatorPreyEntity predPreyEntity)
            return;

        var entityType = predPreyEntity.EntityType();
        if (_predatorTypes.Contains(entityType))
            _predators.Add(entity);
        if (_preyTypes.Contains(entityType))
            _prey.Add(entity);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}