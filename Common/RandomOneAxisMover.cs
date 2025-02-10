using System.Collections.Generic;
using CozyGame.Interface;

namespace CozyGame.Common;

public class RandomOneAxisMover : IOneAxisMover
{
    private readonly IEnumerator<float?> _enumerator;
    private readonly float _maxOutput;
    private readonly int _maxTicksPerMove;
    private readonly float _minOutput;
    private readonly int _minTicksPerMove;

    // TODO: Implement IDisposable and Dispose the IEnumerator<>?

    public RandomOneAxisMover(int minTicksPerMove, int maxTicksPerMove, float minOutput, float maxOutput)
    {
        _minTicksPerMove = minTicksPerMove;
        _maxTicksPerMove = maxTicksPerMove;
        _minOutput = minOutput;
        _maxOutput = maxOutput;
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

        moveValue = 0f;
        return false;
    }

    private IEnumerator<float?> Enumerator()
    {
        while (true) // infinite iterator
        {
            var moveDurationTicks = (int)RandomScalar.GeneratePositive(_minTicksPerMove, _maxTicksPerMove);
            for (var i = 0; i < moveDurationTicks; i++) yield return null;

            yield return RandomScalar.Generate(_minOutput, _maxOutput);
        }
    }
}