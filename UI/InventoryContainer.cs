using Godot;
using KentingStation.Common;

namespace KentingStation.UI;

public partial class InventoryContainer : BoxContainer
{
    public readonly Inventory Inventory = new();
    public readonly int NumCols = 9;
    public readonly int NumRows = 1;
    [Export] public PackedScene InventoryButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var buttonArray = new InventoryButton[NumRows, NumCols];

        for (var r = 0; r < NumRows; r++)
        for (var c = 0; c < NumCols; c++)
        {
            var button = InventoryButton.Instantiate<InventoryButton>();
            button.Name = $"InventoryButton{c}";
            buttonArray[r, c] = button;
            button.Register(Inventory, new InventoryButtonId(r, c));
            AddChild(button);
        }

        Inventory.RegisterButtonArray(buttonArray);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}