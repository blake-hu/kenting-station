using System;
using System.Collections.Frozen;
using System.Collections.Immutable;
using Godot;
using KentingStation.Common.Util;
using KentingStation.Interface;
using KentingStation.Item;
using KentingStation.Item.Instance;

namespace KentingStation.Entity.Instance;

// TODO: Turn grass into Area2D
public partial class Grass : Area2D, IPredatorPreyEntity, ITrackedEntity<Grass>, IDisplayDebugInfo
{
    private const int HealthDecayTimerMax = 100;
    private const int HealthDecayTimerMin = 20;

    [Export] [DebugInfo("HP")] public int _currentHealth;
    private EntityContainer<Grass> _entityContainer;
    [Export] public int BaseHealth;
    [Export] public int MaxHealth;

    private ImmutableHashSet<Type> Predators { get; } =
    [
        typeof(Cow)
    ];

    private ImmutableHashSet<Type> Prey { get; } = [];
    public RandomDelay HealthDecayTimer { get; set; }
    public Label DebugLabel { get; set; }

    public Type EntityType()
    {
        return typeof(Grass);
    }

    public FrozenSet<Type> PredatorTypes()
    {
        return Predators.ToFrozenSet();
    }

    public FrozenSet<Type> PreyTypes()
    {
        return Prey.ToFrozenSet();
    }

    public void IncreaseHealth(int healthPoints)
    {
        _currentHealth += healthPoints;
        if (_currentHealth > MaxHealth)
            _currentHealth = MaxHealth;
    }

    public void DecreaseHealth(int healthPoints)
    {
        _currentHealth -= healthPoints;
        if (_currentHealth < 0)
            Die();
    }

    public void Die()
    {
        // Sometimes Die() is called more than once in the same frame
        if (IsQueuedForDeletion()) return;

        DieCustomLogic();
        if (!_entityContainer.TryRemoveEntity(this))
            throw new System.Exception(
                $"Internal error: Unable to remove entity {Name} from entity container {_entityContainer.GetType()} on death.");
        QueueFree();
    }

    public void RegisterEntityContainer(EntityContainer<Grass> container)
    {
        _entityContainer = container;
    }

    private void UpdateHealth()
    {
        if (HealthDecayTimer.Done())
        {
            _currentHealth--;
            if (_currentHealth <= 0)
                Die();
        }
    }

    private void DieCustomLogic()
    {
        var item = ItemProvider.Singleton.Get<Hay>();
        ItemDropService.Singleton.QueueSpawn(item, 1, Position);
    }

    public override void _Ready()
    {
        _currentHealth = BaseHealth;
        HealthDecayTimer = new RandomDelay(HealthDecayTimerMin, HealthDecayTimerMax);
        DebugLabel = (this as IDisplayDebugInfo).SetupDebugInfo();
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateHealth();

        (this as IDisplayDebugInfo).UpdateDebugInfo(DebugLabel);
    }
}