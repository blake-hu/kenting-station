using System.Collections.Generic;
using Godot;
using KentingStation.Common.Util;
using KentingStation.Interface;
using WorldBoundary = KentingStation.Common.WorldBoundary;

namespace KentingStation.Item;

public partial class ItemDropService : Node2D
{
    private readonly List<ItemSpawnInfo> _spawnQueue = [];
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
        if (_spawnQueue.Count == 0)
            return;
        foreach (var itemInfo in _spawnQueue)
            Spawn(itemInfo);
        _spawnQueue.Clear();
    }

    // We use QueueSpawn instead of Die here. This added complexity is needed because:
    // - Spawning is often called by Die in an event handler context
    // - Spawning an ItemDrop involves instantiating an Area2D in the scene tree
    // - The Godot physics engine does not allow modifying an Area2D while in an event handler
    // - Hence, we need to queue the spawn request and only handle it once we exit the event handler
    public void QueueSpawn(IItem item, int count, Vector2 spawnLocation)
    {
        _spawnQueue.Add(new ItemSpawnInfo(item, count, spawnLocation));
    }

    private void Spawn(ItemSpawnInfo itemInfo)
    {
        var itemDrop = _itemDrop.Instantiate<ItemDrop>();
        itemDrop.SetItem(itemInfo.Item, itemInfo.Count);

        var boundedSpawnLocation = Rect2Ex.ClosestPointWithin(WorldBoundary.Singleton.Boundary, itemInfo.SpawnLocation);
        itemDrop.Position = boundedSpawnLocation;
        Singleton.AddChild(itemDrop);
    }

    private record struct ItemSpawnInfo(IItem Item, int Count, Vector2 SpawnLocation);
}