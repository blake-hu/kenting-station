using Godot;
using Kenting.Common;
using Kenting.Entity;
using Kenting.Interface;

namespace KentingStation.Entity;

public partial class Tiger : CharacterBody2D, ITrackedEntity<Tiger>, IFreeze
{
    private AnimatedSprite2D _animatedSprite2D;
    private EntityContainer<Tiger> _entityContainer;
    private bool _frozen;
    private RandomOneAxisMover _randomOneAxisXMover;
    private RandomOneAxisMover _randomOneAxisYMover;

    [Export] public float DiagonalWalk;
    [Export] public int MaxRunDuration = 50;
    [Export] public float MaxRunSpeed = 80f;
    [Export] public int MaxWalkDuration = 100;
    [Export] public float MaxXWalkSpeed = 20f;
    [Export] public float MaxYWalkSpeed = 10f;
    [Export] public int MinRunDuration = 20;
    [Export] public float MinRunSpeed = 30f;
    [Export] public int MinWalkDuration = 20;

    public bool Freeze()
    {
        _frozen = true;
        _animatedSprite2D?.Stop();
        return true;
    }

    public bool Unfreeze()
    {
        _frozen = false;
        return true;
    }

    public void RegisterEntityContainer(EntityContainer<Tiger> container)
    {
        _entityContainer = container;
    }

    public void Die()
    {
        if (!_entityContainer.TryRemoveEntity(this))
            throw new System.Exception(
                $"Internal error: Unable to remove entity {Name} from entity container {_entityContainer.GetType()} on death.");
        GD.Print($"{Name} was killed");
        QueueFree();
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _randomOneAxisXMover =
            new RandomOneAxisMover(MinWalkDuration, MaxWalkDuration, -MaxXWalkSpeed, MaxXWalkSpeed);
        _randomOneAxisYMover =
            new RandomOneAxisMover(MinWalkDuration, MaxWalkDuration, -MaxYWalkSpeed, MaxYWalkSpeed);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_frozen) return;

        var move = Vector2.Zero;

        if (_randomOneAxisXMover.NextMove(out var randomXMove))
        {
            move.X += randomXMove;
            // For more realistic movement, also move slightly up/down when moving horizontally
            move.Y += randomXMove * RandomScalar.Generate(-DiagonalWalk, DiagonalWalk);
        }

        if (_randomOneAxisYMover.NextMove(out var randomYMove))
        {
            move.Y += randomYMove;
            // For more realistic movement, also move slightly left/right when moving vertically
            move.X += randomYMove * RandomScalar.Generate(-DiagonalWalk, DiagonalWalk);
        }

        if (move != Vector2.Zero)
        {
            MoveX(move.X);
            MoveY(move.Y);
        }

        MoveAndSlide();
    }

    private void MoveX(float speed)
    {
        Velocity = speed * Vector2.Right;
        var scale = _animatedSprite2D.Scale;
        scale.X *= -1; // Invert sprite horizontally
        _animatedSprite2D.Scale = scale;
        _animatedSprite2D.Play("right");
    }

    private void MoveY(float speed)
    {
        Velocity = new Vector2(Velocity.X, speed);
    }
}