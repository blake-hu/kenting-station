using System;
using System.Collections.Generic;
using Kenting.Interface;
using KentingStation.Exception;

namespace KentingStation.Item;

public abstract class ProviderBase<TProduct>
{
    private readonly string _className = nameof(ProviderBase<TProduct>);
    protected abstract Dictionary<Type, IFactory<TProduct>> FactoryDict { get; init; }

    public T Get<T>() where T : class, TProduct
    {
        if (FactoryDict.TryGetValue(typeof(T), out var itemFactory))
        {
            var genericItem = itemFactory.GetInstance();
            if (genericItem is T item)
                return item;
            throw new KsInvalidCastException(_className, nameof(genericItem), typeof(T).ToString(),
                $"This is likely because the dictionary in {_className} was not properly configured.");
        }

        throw new KsKeyNotFoundException(_className, typeof(T).ToString(), FactoryDict);
    }
}