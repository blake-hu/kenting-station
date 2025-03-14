using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using Godot;
using KentingStation.Common.Util;
using KentingStation.Interface;

// Detect when a predator is within a specified radius of the parent entity
// and kills the parent entity
namespace KentingStation.Entity;

public partial class AttackRange : Area2D
{
	private readonly List<IPredatorPreyEntity> _preyInRange = new();
	private readonly Random _random = new();
	private RandomDelay _attackDelay;
	private FrozenSet<Type> _preyTypes;
	[Export] public int AttackDamage;
	[Export] public int AttackDelayMax;
	[Export] public int AttackDelayMin;
	[Export] public float DetectionRadius;

	public override void _Ready()
	{
		var parentNode = GetParent();
		if (parentNode is not IPredatorPreyEntity predPreyEntity)
			throw new System.Exception($"Parent of {nameof(AttackRange)} must be {nameof(IPredatorPreyEntity)}");
		_preyTypes = predPreyEntity.PreyTypes();

		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		if (collisionShape.Shape is not CircleShape2D circle)
			throw new System.Exception(
				$"{nameof(CollisionShape2D)} must be a {nameof(CircleShape2D)} in {nameof(AttackRange)}");
		circle.Radius = DetectionRadius;

		_attackDelay = new RandomDelay(AttackDelayMin, AttackDelayMax);

		BodyEntered += OnNodeEntered;
		BodyExited += OnNodeExited;
		// These methods below work because of contravariance in delegates
		AreaEntered += OnNodeEntered;
		AreaExited += OnNodeExited;
	}

	public override void _Process(double delta)
	{
		if (_preyInRange.Count == 0)
			return;
		if (!_attackDelay.Done())
			return;
		var randomIndex = _random.Next(0, _preyInRange.Count);
		_preyInRange[randomIndex].DecreaseHealth(AttackDamage);
	}

	private bool IsPrey(Node2D body)
	{
		return body is IPredatorPreyEntity predPreyEntity
			   && _preyTypes.Contains(predPreyEntity.EntityType());
	}

	private void OnNodeEntered(Node2D body)
	{
		if (IsPrey(body))
			_preyInRange.Add(body as IPredatorPreyEntity);
	}

	private void OnNodeExited(Node2D body)
	{
		if (IsPrey(body))
			_preyInRange.Remove(body as IPredatorPreyEntity);
	}
}
