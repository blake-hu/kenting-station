using System;
using System.Collections.Generic;
using Kenting.Common;
using Kenting.Interface;

namespace KentingStation.Item;

public class ItemProvider : ProviderBase<IItem>
{
    public static ItemProvider Singleton { get; } = new();

    protected override Dictionary<Type, IFactory<IItem>> FactoryDict { get; init; } = new()
    {
        { typeof(Beef), new LazySingletonFactory<Beef>() },
        { typeof(Sword), new NewInstanceFactory<Sword>() }
    };
}