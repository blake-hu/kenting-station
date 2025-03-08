using System.Collections.Generic;

namespace KentingStation.Common.Util;

public class RandomDelay
{
    private readonly IEnumerator<bool> _enumerator;
    private readonly int _maximum;
    private readonly int _minimum;

    public RandomDelay(int constantDelay)
    {
        _minimum = constantDelay;
        _maximum = constantDelay;
        _enumerator = Enumerator();
    }

    public RandomDelay(int minimum, int maximum)
    {
        _minimum = minimum;
        _maximum = maximum;
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
            var randomDelay = (int)RandomFloat.GeneratePositive(_minimum, _maximum);
            for (var i = 0; i < randomDelay; i++)
                yield return false;

            yield return true;
        }
    }
}