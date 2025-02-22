using Godot;
using KentingStation.Common.Util;
using KentingStation.Interface;
using WorldBoundary = KentingStation.Common.WorldBoundary;

namespace KentingStation.Item;

public partial class ItemDropService : Node2D
{
    private PackedScene _itemDrop = ResourceLoader.Load<PackedScene>("res://Scene/ItemDrop.tscn");

    private ItemDropService()
    {
    }

    public static ItemDropService Singleton { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Singleton = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void Spawn(IItem item, int count, Vector2 spawnLocation)
    {
        var itemDrop = _itemDrop.Instantiate<ItemDrop>();
        itemDrop.SetItem(item, count);

        var boundedSpawnLocation = Rect2Ex.ClosestPointWithin(WorldBoundary.Singleton.Boundary, spawnLocation);
        itemDrop.Position = boundedSpawnLocation;
        Singleton.AddChild(itemDrop);
    }
}