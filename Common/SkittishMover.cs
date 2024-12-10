using CozyGame.Interface;
using Godot;

namespace CozyGame.scene;

public class SkittishMover : IMoveTwoAxis
{
    private readonly CharacterBody2D _character;
    private readonly RandomOneAxisMover _randomOneAxisMover;
    private readonly float _triggerRadius;

    public SkittishMover(
        CharacterBody2D character,
        float triggerRadius,
        int minTicksPerRun,
        int maxTicksPerRun,
        float minRunSpeed,
        float maxRunSpeed)
    {
        _randomOneAxisMover = new RandomOneAxisMover(minTicksPerRun, maxTicksPerRun, minRunSpeed, maxRunSpeed);
        _triggerRadius = triggerRadius;
        _character = character;
    }

    public bool NextMove(out Vector2 moveValues)
    {
        moveValues = Vector2.Zero; // default
        var playerPosition = Player.GetPlayerPosition();
        var characterPosition = _character.Position;
        var playerToCharacter = characterPosition - playerPosition;
        var distance = playerToCharacter.Length();
        if (distance > _triggerRadius) return false;

        if (_randomOneAxisMover.NextMove(out var speed))
        {
            moveValues = playerToCharacter.Normalized() * speed;
            GD.Print($"Skittish move: {moveValues}");
            return true;
        }

        return false;
    }
}