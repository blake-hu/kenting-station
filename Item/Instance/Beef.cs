using System;
using System.Collections.Frozen;
using System.Collections.Immutable;
using Godot;
using KentingStation.Entity.Instance;
using KentingStation.Interface;
using Player = KentingStation.Entity.Instance.Player;

namespace KentingStation.Item.Instance;

public class Beef : IItem
{
    private const int HealthPoints = 40;

    private readonly ImmutableHashSet<Type> _canPickUp =
    [
        typeof(Tiger)
    ];

    private readonly Texture2D _texture;

    public Beef() // Do not construct directly, use ItemProvider instead
    {
        _texture = ResourceLoader.Load<CompressedTexture2D>("res://Asset/CadmiumRedMeatPixelArt/beef-16px.png");
    }

    public ItemType ItemType => ItemType.Beef;

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

    public int MaxCountPerStack => 64;
}