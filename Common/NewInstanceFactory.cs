using KentingStation.Interface;

namespace KentingStation.Common;

public class NewInstanceFactory<T> : IFactory<T> where T : new()
{
    public T GetInstance()
    {
        return new T();
    }
}