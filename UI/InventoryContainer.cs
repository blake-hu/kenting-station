using Godot;

public partial class InventoryContainer : BoxContainer
{
    public readonly int NumInventorySlots = 9;
    [Export] public PackedScene InventoryButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (var i = 0; i < NumInventorySlots; i++)
        {
            var buttonScene = InventoryButton.Instantiate();
            buttonScene.Name = $"InventoryButton{i}";
            AddChild(buttonScene);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}