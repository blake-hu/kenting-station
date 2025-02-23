using KentingStation.Common.Util;
using KentingStation.Interface;

namespace KentingStation.Entity.Mover;

public class RandomOneAxisMover : IOneAxisMover
{
    private readonly float _maxOutput;
    private readonly float _minOutput;
    private readonly RandomDelay _randomDelay;

    public RandomOneAxisMover(int minTicksPerMove, int maxTicksPerMove, float minOutput, float maxOutput)
    {
        _minOutput = minOutput;
        _maxOutput = maxOutput;
        _randomDelay = new RandomDelay(minTicksPerMove, maxTicksPerMove);
    }

    public bool NextMove(out float moveValue)
    {
        if (_randomDelay.Done())
        {
            moveValue = RandomScalar.Generate(_minOutput, _maxOutput);
            return true;
        }

        moveValue = 0f;
        return false;
    }
}