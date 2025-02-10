using System.Collections.Generic;
using Godot;

namespace CozyGame.Common;

public static class CharacterGroupEx
{
    public static CharacterBody2D FindClosestCharacter(IReadOnlyCollection<CharacterBody2D> characterGroup,
        Vector2 target)
    {
        CharacterBody2D closestCharacter = null;
        var closestDistance = float.MaxValue;

        foreach (var character in characterGroup)
        {
            var distance = (character.Position - target).Length();
            if (distance < closestDistance)
            {
                closestCharacter = character;
                closestDistance = distance;
            }
        }

        return closestCharacter;
    }
}