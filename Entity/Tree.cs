using System;
using CozyGame.Interface;
using Godot;

namespace CozyGame.Entity;

public partial class Tree : StaticBody2D, IEntity<Tree>
{
	private EntityContainer<Tree> _entityContainer;

	public void RegisterEntityContainer(EntityContainer<Tree> container)
	{
		_entityContainer = container;
	}

	public void Die()
	{
		if (!_entityContainer.TryRemoveEntity(this))
			throw new Exception(
				$"Internal error: Unable to remove entity {Name} from entity container {_entityContainer.GetType()} on death.");
		GD.Print($"{Name} was killed");
		QueueFree();
	}
}
