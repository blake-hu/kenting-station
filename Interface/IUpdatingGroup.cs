using System.Collections.Generic;
using Godot;

namespace Kenting.Interface;

public interface IUpdatingGroup<T> where T : Node2D
{
    public IReadOnlyCollection<T> GetUpdatedCharacters();
}