using System;
using System.Collections.Frozen;

namespace KentingStation.Interface;

public interface IPredatorPreyEntity
{
    public Type EntityType();
    public FrozenSet<Type> PredatorTypes();
    public FrozenSet<Type> PreyTypes();
    public void IncreaseHealth(int healthPoints);
    public void DecreaseHealth(int healthPoints);
}