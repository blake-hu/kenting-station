using System.Collections.Generic;
using Kenting.Interface;
using KentingStation.Exception;

namespace KentingStation.Item;

public abstract class ProviderBase<TProductId, TProduct>
{
    private readonly string _className = nameof(ProviderBase<TProductId, TProduct>);
    protected abstract Dictionary<TProductId, IFactory<TProduct>> FactoryDict { get; init; }

    public TProduct Get(TProductId id)
    {
        if (FactoryDict.TryGetValue(id, out var itemFactory))
        {
            var genericItem = itemFactory.GetInstance();
            return genericItem;
        }

        throw new KsKeyNotFoundException(_className, id.ToString(), FactoryDict);
    }

    public T Get<T>(TProductId id) where T : class, TProduct
    {
        var genericItem = Get(id);
        if (genericItem is T item)
            return item;
        throw new KsInvalidCastException(_className, nameof(genericItem), typeof(T).ToString(),
            $"This is likely because the dictionary in {_className} was not properly configured.");
    }
}