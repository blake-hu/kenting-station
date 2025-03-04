using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using Godot;
using KentingStation.Item;
using Player = KentingStation.Entity.Instance.Player;

namespace KentingStation.Interface;

public interface IItem
{
    public ItemType ItemType();

    public int MaxCountPerStack();

    public Texture2D GetDisplayTexture();

    public FrozenSet<Type> PickedUpBy()
    {
        return new HashSet<Type>().ToFrozenSet(); // default behavior: cannot be picked up by non-Players
    }

    public bool LeftClick(Player player)
    {
        return true; // default behavior: clicking has no effect
    }

    public bool RightClick(Player player)
    {
        return true; // default behavior: clicking has no effect
    }
}