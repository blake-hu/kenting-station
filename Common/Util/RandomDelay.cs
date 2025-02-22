using System.Collections.Generic;
using Kenting.Common;

namespace KentingStation.Common;

public class RandomDelay
{
    private readonly IEnumerator<bool> _enumerator;
    private readonly int _maxTicksPerMove;
    private readonly int _minTicksPerMove;

    public RandomDelay(int minTicksPerMove, int maxTicksPerMove)
    {
        _minTicksPerMove = minTicksPerMove;
        _maxTicksPerMove = maxTicksPerMove;
        _enumerator = Enumerator();
    }

    public bool Done()
    {
        _enumerator.MoveNext();
        return _enumerator.Current;
    }

    private IEnumerator<bool> Enumerator()
    {
        while (true) // infinite enumerator
        {
            var randomDelay = (int)RandomScalar.GeneratePositive(_minTicksPerMove, _maxTicksPerMove);
            for (var i = 0; i < randomDelay; i++)
                yield return false;

            yield return true;
        }
    }
}