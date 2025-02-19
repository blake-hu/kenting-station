using System;
using System.Collections.Generic;
using Kenting.Common;
using Kenting.Interface;

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
        if (_db.TryGetValue(typeof(ItemId), out var itemFactory))
        {
            var genericItem = itemFactory.GetInstance();
            if (genericItem is TItem item)
                return item;
            throw new Exception(
                $"Internal error: Could not cast generic item {genericItem} to concrete item type {typeof(TItem)}." +
                $"This is likely because {nameof(ItemProvider)} is not correctly configured.");
        }

        throw new KeyNotFoundException($"Item of type {typeof(TItem)} not found in {nameof(ItemProvider)}");
    }
}