using System.Collections.Generic;
using Godot;
using Kenting.Interface;

namespace Kenting.Common;

public class SkittishMover : ITwoAxisMover
{
	private readonly IList<IUpdatingGroup<CharacterBody2D>> _enemies;
	private readonly RandomOneAxisMover _randomOneAxisMover;
	private readonly CharacterBody2D _skittishCharacter;
	private readonly float _triggerRadius;

	public SkittishMover(
		CharacterBody2D skittishCharacter,
		List<IUpdatingGroup<CharacterBody2D>> enemies,
		float triggerRadius,
		int minTicksPerRun,
		int maxTicksPerRun,
		float minRunSpeed,
		float maxRunSpeed)
	{
		_randomOneAxisMover =
			new RandomOneAxisMover(minTicksPerRun, maxTicksPerRun, minRunSpeed, maxRunSpeed);
		_triggerRadius = triggerRadius;
		_skittishCharacter = skittishCharacter;
		_enemies = enemies;
	}

	public bool NextMove(out Vector2 moveValues)
	{
		moveValues = Vector2.Zero; // default
		var closestEnemy = ClosestEnemy();
		if (closestEnemy == null) return false;
		var enemyPosition = closestEnemy.Position;
		var skittishPosition = _skittishCharacter.Position;
		var enemyToSkittish = skittishPosition - enemyPosition;
		var distance = enemyToSkittish.Length();
		if (distance > _triggerRadius) return false;

		if (_randomOneAxisMover.NextMove(out var speed))
		{
			moveValues = enemyToSkittish.Normalized() * speed;
			return true;
		}

		return false;
	}

	private CharacterBody2D ClosestEnemy()
	{
		var target = _skittishCharacter.Position;
		CharacterBody2D closestEnemy = null;
		var closestDistance = float.MaxValue;

		foreach (var enemyGroup in _enemies)
		{
			var latestEnemies = enemyGroup.GetUpdatedCharacters();
			var closestEnemyInGroup = CharacterGroupEx.FindClosestCharacter(latestEnemies, target);
			if (closestEnemyInGroup == null) continue;
			var distanceToTarget = (closestEnemyInGroup.Position - target).Length();
			if (distanceToTarget < closestDistance)
			{
				closestEnemy = closestEnemyInGroup;
				closestDistance = distanceToTarget;
			}
		}

		return closestEnemy;
	}
}
