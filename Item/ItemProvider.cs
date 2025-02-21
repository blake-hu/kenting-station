using System.Collections.Generic;
using Kenting.Common;
using Kenting.Interface;

namespace KentingStation.Item;

public class ItemProvider : ProviderBase<ItemType, IItem>
{
    public static ItemProvider Singleton { get; } = new();

    protected override Dictionary<ItemType, IFactory<IItem>> FactoryDict { get; init; } = new()
    {
        { ItemType.Beef, new LazySingletonFactory<Beef>() },
        { ItemType.Sword, new NewInstanceFactory<Sword>() }
    };
}