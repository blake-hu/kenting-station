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
    private readonly Texture2D _texture;

    private readonly ImmutableHashSet<Type> _pickedUpBy =
    [
        typeof(Player),
        typeof(Tiger)
    ];

    public Beef() // Do not construct directly, use ItemProvider instead
    {
        // TODO Low Priority: Find beef texture
        _texture = ResourceLoader.Load<CompressedTexture2D>("res://Asset/CadmiumRedMeatPixelArt/beef-16px.png");
    }

    public ItemType ItemType()
    {
        return Item.ItemType.Beef;
    }

    public int MaxCountPerStack()
    {
        return 64;
    }

    public Texture2D GetDisplayTexture()
    {
        return _texture;
    }

    public FrozenSet<Type> PickedUpBy()
    {
        return _pickedUpBy.ToFrozenSet();
    }

    public bool RightClick(Player player)
    {
        return true;
    }
}