using System;
using System.Collections.Generic;
using Kenting.Common;
using Kenting.Interface;
using KentingStation.Exception;

namespace KentingStation.Item;

public class ItemProvider
{
    private readonly Dictionary<Type, IFactory<IItem>> _db = new()
    {
        { typeof(Beef), new LazySingletonFactory<Beef>() },
        { typeof(Sword), new NewInstanceFactory<Sword>() }
    };

    private ItemProvider()
    {
    }

    public static ItemProvider Singleton { get; } = new();

    public TItem GetItem<TItem>() where TItem : class, IItem
    {
        if (_db.TryGetValue(typeof(TItem), out var itemFactory))
        {
            var genericItem = itemFactory.GetInstance();
            if (genericItem is TItem item)
                return item;
            throw new KsInvalidCastException(nameof(ItemProvider), nameof(genericItem), typeof(TItem).ToString(),
                $"This is likely because the dictionary in {nameof(ItemProvider)} was not properly configured.");
        }

        throw new KsKeyNotFoundException(nameof(ItemProvider), typeof(TItem).ToString(), _db);
    }
}