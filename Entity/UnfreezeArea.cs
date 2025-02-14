using CozyGame.Interface;
using Godot;

public partial class UnfreezeArea : Area2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Entities spawned within UnfreezeArea will correctly trigger BodyEntered event
        BodyEntered += OnBodyEntered;

        // However, entities that are spawned outside UnfreezeArea do not trigger BodyExited event
        // To work around this, all IFreeze entities should start out frozen
        // EntityService.Spawn() enforces this rule
        BodyExited += OnBodyExited;

        Monitoring = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private static void OnBodyEntered(Node2D body)
    {
        if (body is IFreeze frozenBody) frozenBody.Unfreeze();
    }

    private static void OnBodyExited(Node2D body)
    {
        if (body is IFreeze unfrozenBody) unfrozenBody.Freeze();
    }
}