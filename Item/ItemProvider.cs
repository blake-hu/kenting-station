using System;
using System.Collections.Generic;
using KentingStation.Common;
using KentingStation.Interface;
using KentingStation.Item.Instance;

namespace KentingStation.Item;

public class ItemProvider : ProviderBase<IItem>
{
    public static ItemProvider Singleton { get; } = new();

    protected override Dictionary<Type, IFactory<IItem>> FactoryDict { get; init; } = new()
    {
        { typeof(Beef), new LazySingletonFactory<Beef>() },
        { typeof(Sword), new NewInstanceFactory<Sword>() },
        { typeof(Hay), new LazySingletonFactory<Hay>() }
    };
}