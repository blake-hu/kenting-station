using System;

namespace KentingStation.Common.Util;

public static class RandomFloat
{
    private static readonly Random Rng = new();

    public static float Generate(float min, float max)
    {
        IsValidRange(min, max);
        return GenerateInternal(min, max);
    }

    public static float GeneratePositive(float min, float max)
    {
        IsPositiveRange(min, max);
        return GenerateInternal(min, max);
    }

    private static float GenerateInternal(float min, float max)
    {
        var range = max - min;
        return (float)Rng.NextDouble() * range + min;
    }

    private static void IsValidRange(float min, float max)
    {
        if (min > max)
            throw new ArgumentException(
                $"RandomScalar: Min value {min} must be smaller than max value {max}.");
    }

    private static void IsPositiveRange(float min, float max)
    {
        IsValidRange(min, max);
        if (min < 0.0f)
            throw new ArgumentException(
                $"RandomScalar.GeneratePositive: Min value {min} must be positive.");
    }
}