using System;
using System.Collections.Frozen;
using Godot;
using KentingStation.Interface;

// Detect when a predator is within a specified radius of the parent entity
// and kills the parent entity
namespace KentingStation.Entity;

public partial class AttackRange : Area2D
{
    private IKillableEntity _parentEntity;
    private FrozenSet<Type> _predatorTypes;
    [Export] public float DetectionRadius;

    public override void _Ready()
    {
        var parentNode = GetParent();
        if (parentNode is not IPredatorPreyEntity predPreyEntity)
            throw new System.Exception($"Parent of {nameof(AttackRange)} must be {nameof(IPredatorPreyEntity)}");
        _predatorTypes = predPreyEntity.PredatorTypes();

        if (parentNode is not IKillableEntity parentEntity)
            throw new System.Exception($"Parent of {nameof(AttackRange)} must be {nameof(IKillableEntity)}");
        _parentEntity = parentEntity;

        var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        if (collisionShape.Shape is not CircleShape2D circle)
            throw new System.Exception(
                $"{nameof(CollisionShape2D)} must be a {nameof(CircleShape2D)} in {nameof(AttackRange)}");
        circle.Radius = DetectionRadius;

        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not CharacterBody2D entity)
            return;
        if (entity is not IPredatorPreyEntity predPreyEntity)
            return;

        var entityType = predPreyEntity.EntityType();
        if (_predatorTypes.Contains(entityType))
            _parentEntity.Die();
    }
}