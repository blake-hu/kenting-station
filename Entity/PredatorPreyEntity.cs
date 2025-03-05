using System;
using System.Collections.Frozen;
using System.Collections.Immutable;
using Godot;
using KentingStation.Common.Util;
using KentingStation.Entity.Mover;
using KentingStation.Exception;
using KentingStation.Interface;
using PredatorPreyMover = KentingStation.Entity.Mover.PredatorPreyMover;

namespace KentingStation.Entity;

public partial class PredatorPreyEntity<TEntity> : CharacterBody2D, IPredatorPreyEntity, ITrackedEntity<TEntity>,
    IDisplayDebugInfo
    where TEntity : CharacterBody2D
{
    private const int HealthDecayTimerMin = 10;
    private const int HealthDecayTimerMax = 50;
    private int _age;
    [DebugInfo("AgeDecay")] public int _ageDecayMultiplier = 1;

    private AnimatedSprite2D _animatedSprite2D;
    private Label _debugLabel;
    private EntityContainer<TEntity> _entityContainer;
    private bool _frozen;
    private RandomDelay _healthDecayTimer;
    private PredatorPreyMover _predatorPreyMover;
    private RandomOneAxisMover _randomOneAxisXMover;
    private RandomOneAxisMover _randomOneAxisYMover;

    [Export] public int BaseHealth = 100;
    [DebugInfo("HP")] protected int CurrentHealth;
    [Export] public int MaxHealth;

    // Default values simulate movement of cow, can be overwritten by users in Godot
    [Export] public int RunDurationMax = 50;
    [Export] public int RunDurationMin = 20;
    [Export] public float RunSpeedMax = 80f;
    [Export] public float RunSpeedMin = 30f;

    [Export] public float WalkDiagonal = 0.2f;
    [Export] public int WalkDurationMax = 100;
    [Export] public int WalkDurationMin = 20;
    [Export] public Vector2 WalkSpeedMax = new(10f, 20f);

    // By default, the entity has no prey or predators
    // Override these properties to add prey or predators
    protected virtual ImmutableHashSet<Type> Prey { get; } = [];
    protected virtual ImmutableHashSet<Type> Predators { get; } = [];

    public FrozenSet<Type> PreyTypes()
    {
        return Prey.ToFrozenSet();
    }

    public void IncreaseHealth(int healthPoints)
    {
        CurrentHealth += healthPoints;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public void DecreaseHealth(int healthPoints)
    {
        CurrentHealth -= healthPoints;
        if (CurrentHealth < 0)
            Die();
    }

    public Type EntityType()
    {
        return typeof(TEntity);
    }

    public FrozenSet<Type> PredatorTypes()
    {
        return Predators.ToFrozenSet();
    }

    public void RegisterEntityContainer(EntityContainer<TEntity> container)
    {
        _entityContainer = container;
    }

    public void Die()
    {
        DieCustomLogic();
        if (this is not TEntity derivedEntity)
            throw new KsInvalidCastException(nameof(Die), nameof(PredatorPreyEntity<TEntity>), nameof(TEntity),
                "Likely because entity inheritance hierarchy was not set up correctly.");
        if (!_entityContainer.TryRemoveEntity(derivedEntity))
            throw new System.Exception(
                $"Internal error: Unable to remove entity {Name} from entity container {_entityContainer.GetType()} on death.");
        QueueFree();
    }


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

    protected virtual void DieCustomLogic()
    {
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _randomOneAxisXMover =
            new RandomOneAxisMover(WalkDurationMin, WalkDurationMax, -WalkSpeedMax.X, WalkSpeedMax.X);
        _randomOneAxisYMover =
            new RandomOneAxisMover(WalkDurationMin, WalkDurationMax, -WalkSpeedMax.Y, WalkSpeedMax.Y);
        _predatorPreyMover = GetNode<PredatorPreyMover>("PredatorPreyMover");
        CurrentHealth = BaseHealth;
        _healthDecayTimer = new RandomDelay(HealthDecayTimerMin, HealthDecayTimerMax);
        _debugLabel = (this as IDisplayDebugInfo).SetupDebugInfo();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_frozen) return;

        var move = ComputeMove();
        if (move != Vector2.Zero)
        {
            MoveX(move.X);
            MoveY(move.Y);
        }

        MoveAndSlide();

        UpdateHealth();

        (this as IDisplayDebugInfo).UpdateDebugInfo(_debugLabel);
    }

    private Vector2 ComputeMove()
    {
        var move = Vector2.Zero;

        if (_predatorPreyMover.NextMove(out var skittishMove))
            move += skittishMove;

        if (_randomOneAxisXMover.NextMove(out var randomXMove))
        {
            move.X += randomXMove;
            // For more realistic movement, also move slightly up/down when moving horizontally
            move.Y += randomXMove * RandomScalar.Generate(-WalkDiagonal, WalkDiagonal);
        }

        if (_randomOneAxisYMover.NextMove(out var randomYMove))
        {
            move.Y += randomYMove;
            // For more realistic movement, also move slightly left/right when moving vertically
            move.X += randomYMove * RandomScalar.Generate(-WalkDiagonal, WalkDiagonal);
        }

        // Scale movement by amount of health left
        move *= CurrentHealth / (float)BaseHealth;

        return move;
    }


    private void UpdateHealth()
    {
        if (_healthDecayTimer.Done())
        {
            CurrentHealth -= _ageDecayMultiplier;
            if (CurrentHealth <= 0)
                Die();

            _age++;
            if (_age % BaseHealth == 0)
                _ageDecayMultiplier++;
        }
    }

    private void MoveX(float speed)
    {
        Velocity = speed * Vector2.Right;
        var scale = _animatedSprite2D.Scale;
        var absX = Math.Abs(scale.X);
        if (speed > 0f)
            scale.X = absX;
        else
            scale.X = -absX; // Flip sprite horizontally
        _animatedSprite2D.Scale = scale;

        _animatedSprite2D.Play("right");
    }

    private void MoveY(float speed)
    {
        Velocity = new Vector2(Velocity.X, speed);
    }
}