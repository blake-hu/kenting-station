using System;
using System.Collections.Generic;
using KentingStation.Interface;
using KentingStation.Item;
using KentingStation.Item.Instance;

namespace KentingStation.Entity.Instance;

public partial class Cow : PredatorPreyEntity<Cow>, IFreeze
{
    protected override HashSet<Type> Predators { get; } =
    [
        typeof(Tiger),
        typeof(Player)
    ];

    protected override void DieCustomLogic()
    {
        var beef = ItemProvider.Singleton.Get<Beef>();
        ItemDropService.Singleton.QueueSpawn(beef, 1, Position);
    }
}