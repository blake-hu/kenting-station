using System;

namespace KentingStation.Exception;

public class KentingInvalidCastException : InvalidCastException
{
    public KentingInvalidCastException(string context, string from, string toType) : base(
        $"{context}: Could not cast generic item {from} to concrete item type {toType}.")
    {
    }

    public KentingInvalidCastException(string context, string from, string toType, string explanation) : base(
        $"{context}: Could not cast generic item {from} to concrete item type {toType}. {explanation}")
    {
    }
}