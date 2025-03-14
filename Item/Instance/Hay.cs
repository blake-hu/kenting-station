using System;
using System.Collections.Frozen;
using System.Collections.Immutable;
using Godot;
using KentingStation.Entity.Instance;
using KentingStation.Interface;

namespace KentingStation.Item.Instance;

public class Hay : IItem
{
    private const int HealthPoints = 5;

    private readonly ImmutableHashSet<Type> _canPickUp =
    [
        typeof(Cow)
    ];

    private readonly Texture2D _texture;

    public Hay() // Do not construct directly, use ItemProvider instead
    {
        _texture = ResourceLoader.Load<CompressedTexture2D>("res://Asset/Shutterstock/2149324339/hay.png");
    }

    public ItemType ItemType => ItemType.Hay;

    public int MaxCountPerStack => 64;

    public Texture2D GetDisplayTexture()
    {
        return _texture;
    }

    public FrozenSet<Type> EntitiesThatCanPickUp()
    {
        return _canPickUp.ToFrozenSet();
    }

    public void PickedUpBy(IPredatorPreyEntity entity)
    {
        entity.IncreaseHealth(HealthPoints);
    }

    public bool RightClick(Player player)
    {
        return true;
    }
}