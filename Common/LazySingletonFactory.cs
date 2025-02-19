using System;
using Kenting.Interface;

namespace Kenting.Common;

public class LazySingletonFactory<T> : IFactory<T> where T : new()
{
    private readonly Lazy<T> _singleton = new();

    public T GetInstance()
    {
        return _singleton.Value;
    }
}