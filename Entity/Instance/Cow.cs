using System;
using System.Collections.Immutable;
using KentingStation.Interface;
using KentingStation.Item;
using KentingStation.Item.Instance;

namespace KentingStation.Entity.Instance;

public partial class Cow : PredatorPreyEntity<Cow>, IFreeze
{
    protected override ImmutableHashSet<Type> Predators { get; } =
    [
        typeof(Tiger),
        typeof(Player)
    ];

    protected override ImmutableHashSet<Type> Prey { get; } =
    [
        typeof(Grass)
    ];

    protected override void DieCustomLogic()
    {
        var item = ItemProvider.Singleton.Get<Beef>();
        ItemDropService.Singleton.QueueSpawn(item, 1, Position);
    }
}