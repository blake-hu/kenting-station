using Godot;
using Kenting.Common;
using KentingStation.Exception;

namespace KentingStation.UI;

public partial class InventoryButton : Button
{
    private InventoryButtonId _buttonId;
    private Inventory _inventory;
    public bool IsActive { get; set; }

    public void Register(Inventory inventory, InventoryButtonId buttonId)
    {
        if (_inventory is not null)
            throw new KsReregistrationException(nameof(_inventory));
        _buttonId = buttonId;
        _inventory = inventory;
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