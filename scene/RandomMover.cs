using System;
using System.Collections.Generic;
using CozyGame.Interface;

namespace CozyGame.scene;

public class RandomMover : IMoveOneAxis
{
    private readonly IEnumerator<float?> _enumerator;
    private readonly int _maxTicksPerMove;
    private readonly float _minOutput;
    private readonly int _minTicksPerMove;
    private readonly float _outputRange;
    private readonly Random _rng = new();

    public RandomMover(int minTicksPerMove, int maxTicksPerMove, float minOutput, float maxOutput)
    {
        _minTicksPerMove = minTicksPerMove;
        _maxTicksPerMove = maxTicksPerMove;
        _minOutput = minOutput;
        _outputRange = maxOutput - minOutput;
        _enumerator = Enumerator();
    }

    public bool NextMove(out float moveValue)
    {
        _enumerator.MoveNext();
        if (_enumerator.Current is { } currentValue)
        {
            moveValue = currentValue;
            return true;
        }

        moveValue = -1f;
        return false;
    }

    private IEnumerator<float?> Enumerator()
    {
        while (true) // infinite iterator
        {
            var moveDurationTicks = _rng.Next(_minTicksPerMove, _maxTicksPerMove);
            for (var i = 0; i < moveDurationTicks; i++) yield return null;

            yield return (float)_rng.NextDouble() * _outputRange + _minOutput;
        }
    }
}