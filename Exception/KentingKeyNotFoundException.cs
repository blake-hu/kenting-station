using System.Collections;
using System.Collections.Generic;

namespace KentingStation.Exception;

public class KentingKeyNotFoundException : KeyNotFoundException
{
    public KentingKeyNotFoundException(string context, string key, IDictionary dict) : base(
        $"{context}: Key {key} with type {dict.Keys.GetType()} not found.")
    {
    }

    public KentingKeyNotFoundException(string context, string key, IDictionary dict, string explanation) : base(
        $"{context}: Key {key} with type {dict.Keys.GetType()} not found. {explanation}")
    {
    }
}