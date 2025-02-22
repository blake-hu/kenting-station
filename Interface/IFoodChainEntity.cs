using System;
using System.Collections.Frozen;

namespace KentingStation.Interface;

// TODO NEXT: Call these methods in DetectorCircle on _Ready
public interface IFoodChainEntity
{
    public Type EntityType();
    public FrozenSet<Type> PredatorTypes();

    public FrozenSet<Type> PreyTypes();
}