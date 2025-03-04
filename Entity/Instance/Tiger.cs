using System;
using System.Collections.Generic;
using KentingStation.Interface;

namespace KentingStation.Entity.Instance;

public partial class Tiger : PredatorPreyEntity<Tiger>, IFreeze
{
    protected override HashSet<Type> Prey { get; } =
    [
        typeof(Cow),
        typeof(Player)
    ];
}