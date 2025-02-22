using System;
using System.Collections.Generic;
using Godot;
using Kenting.Interface;
using KentingStation.Entity;
using KentingStation.Item;

namespace Kenting.Entity;

public partial class Cow : PredatorPreyEntity<Cow>, ITrackedEntity<Cow>, IFreeze
{
    protected override HashSet<Type> Predators { get; } =
    [
        typeof(Tiger),
        typeof(Player)
    ];

    protected override void DieCustomLogic()
    {
        var beef = ItemProvider.Singleton.Get<Beef>();
        ItemDropService.Singleton.Spawn(beef, 1, Position);
    }
}