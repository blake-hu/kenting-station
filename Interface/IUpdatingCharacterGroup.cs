using System.Collections.Generic;
using Godot;

namespace CozyGame.Interface;

public interface IUpdatingCharacterGroup
{
    public IReadOnlyCollection<CharacterBody2D> GetUpdatedCharacters();
}