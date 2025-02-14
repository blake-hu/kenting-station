using Godot;

public partial class UnfreezeArea : Area2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Entities spawned within UnfreezeArea will correctly trigger BodyEntered event
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        Monitoring = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private static void OnBodyEntered(Node2D body)
    {
        GD.Print($"{body.Name} entered unfreeze area");
    }

    private static void OnBodyExited(Node2D body)
    {
        GD.Print($"{body.Name} exited unfreeze area");
    }
}