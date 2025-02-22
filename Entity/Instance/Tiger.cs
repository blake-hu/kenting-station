using System;
using System.Collections.Generic;
using Kenting.Entity;
using Kenting.Interface;
using KentingStation.Interface;

namespace KentingStation.Entity;

public partial class Tiger : PredatorPreyEntity<Tiger>, ITrackedEntity<Tiger>, IPredatorPreyEntity, IFreeze
{
    protected override HashSet<Type> Prey { get; } =
    [
        typeof(Cow),
        typeof(Player)
    ];
}