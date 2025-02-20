using System.Collections;
using System.Collections.Generic;

namespace KentingStation.Exception;

public class KsKeyNotFoundException : KeyNotFoundException
{
    public KsKeyNotFoundException(string context, string key, IDictionary dict) : base(
        $"{context}: Key {key} with type {dict.Keys.GetType()} not found.")
    {
    }

    public KsKeyNotFoundException(string context, string key, IDictionary dict, string explanation) : base(
        $"{context}: Key {key} with type {dict.Keys.GetType()} not found. {explanation}")
    {
    }
}