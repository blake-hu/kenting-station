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