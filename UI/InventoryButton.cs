using Godot;
using Kenting.Common;
using KentingStation.Exception;
using KentingStation.Item;

namespace KentingStation.UI;

public partial class InventoryButton : Button
{
    private readonly JarCounter _counter = new();
    private InventoryButtonId _buttonId;
    private Inventory _inventory;
    private IItem _item;
    public bool IsActive { get; set; }
    public int Count => _counter.Count;

    public void Register(Inventory inventory, InventoryButtonId buttonId)
    {
        if (_inventory is not null)
            throw new KsReregistrationException(nameof(_inventory));
        _buttonId = buttonId;
        _inventory = inventory;
    }

    public bool SetItem(IItem newItem, int count, out int overflowAmount)
    {
        overflowAmount = count;
        if (newItem is null)
            return false;
        if (_counter.Count > 0)
            return false; // cannot set to new item if there are still items left
        _counter.ResetMaxCapacity(newItem);
        // TODO: Add logic for handling overflow amount
        _counter.Add(count, out var overflow);
        overflowAmount = overflow;
        _item = newItem;
        Icon = newItem.GetDisplayTexture();
        return true;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ButtonDown += OnButtonDown;
    }

    private void OnButtonDown()
    {
        GD.Print($"Pressed {Name}");
        IsActive = true;
        _inventory.UpdateActiveButton(_buttonId);
    }
}