using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using Godot;
using KentingStation.Item;
using Player = KentingStation.Entity.Instance.Player;

namespace KentingStation.Interface;

public interface IItem
{
    public int MaxCountPerStack { get; }
    public ItemType ItemType { get; }

    public Texture2D GetDisplayTexture();

    public FrozenSet<Type> EntitiesThatCanPickUp()
    {
        return new HashSet<Type>().ToFrozenSet(); // default behavior: cannot be picked up by non-Players
    }

    public void PickedUpBy(IPredatorPreyEntity entity)
    {
    } // default behavior: nothing happens when item picked up by entities that are not Player

    public bool LeftClick(Player player)
    {
        return true; // default behavior: clicking has no effect
    }

    public bool RightClick(Player player)
    {
        return true; // default behavior: clicking has no effect
    }
}