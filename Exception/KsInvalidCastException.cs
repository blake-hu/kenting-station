using System;

namespace KentingStation.Exception;

public class KsInvalidCastException : InvalidCastException
{
    public KsInvalidCastException(string context, string from, string toType) : base(
        $"{context}: Could not cast generic item {from} to concrete item type {toType}.")
    {
    }

    public KsInvalidCastException(string context, string from, string toType, string explanation) : base(
        $"{context}: Could not cast generic item {from} to concrete item type {toType}. {explanation}")
    {
    }
}