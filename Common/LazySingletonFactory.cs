using System;
using KentingStation.Interface;

namespace KentingStation.Common;

public class LazySingletonFactory<T> : IFactory<T> where T : new()
{
    private readonly Lazy<T> _singleton = new();

    public T GetInstance()
    {
        return _singleton.Value;
    }
}