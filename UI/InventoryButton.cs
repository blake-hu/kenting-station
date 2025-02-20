using Godot;

namespace KentingStation.UI;

public partial class InventoryButton : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ButtonDown += () => GD.Print($"Pressed {Name}");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}