using System;
using System.Collections.Immutable;
using KentingStation.Interface;

namespace KentingStation.Entity.Instance;

public partial class Tiger : PredatorPreyEntity<Tiger>, IFreeze
{
    protected override ImmutableHashSet<Type> Prey { get; } =
    [
        typeof(Cow),
        typeof(Player)
    ];
}