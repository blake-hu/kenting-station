using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using Godot;
using KentingStation.Common;
using KentingStation.Interface;
using KentingStation.UI;

namespace KentingStation.Entity.Instance;

public partial class Player : CharacterBody2D, IPredatorPreyEntity
{
    private readonly HashSet<Type> _predators =
    [
        typeof(Tiger)
    ];

    private readonly HashSet<Type> _prey =
    [
        typeof(Cow)
    ];

    private AnimatedSprite2D _animatedSprite2D;
    private Inventory _inventory;
    private RayCast2D _weaponHitDetector;

    [Export] public float Speed = 100.0f;

    public Type EntityType()
    {
        return typeof(Player);
    }

    public FrozenSet<Type> PredatorTypes()
    {
        return _predators.ToFrozenSet();
    }

    public FrozenSet<Type> PreyTypes()
    {
        return _prey.ToFrozenSet();
    }

    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _weaponHitDetector = GetNode<RayCast2D>("WeaponHitDetector");
        var inventoryContainer = GetNode<InventoryContainer>("Hud/InventoryContainer");
        _inventory = inventoryContainer.Inventory;
        OnlinePlayers.RegisterPlayer(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        Move();
        MovementUpdate();
        DetectWeaponHit();
    }

    // TODO: Propagate left and right mouse click to inventory items
    private void DetectWeaponHit()
    {
        if (Input.IsActionJustPressed("mouse_left"))
        {
            var collider = _weaponHitDetector.GetCollider();
            if (collider == null)
                return;
            if (collider is not IKillableEntity entity)
                throw new System.Exception($"Player attacked collider ({collider}) that was not an IEntity");
            entity.QueueDie();
        }
    }

    private void MovementUpdate()
    {
        if (Input.IsActionPressed("left"))
        {
            _animatedSprite2D.Play("left");
            _weaponHitDetector.TargetPosition = new Vector2(-20, 0);
        }
        else if (Input.IsActionPressed("right"))
        {
            _animatedSprite2D.Play("right");
            _weaponHitDetector.TargetPosition = new Vector2(20, 0);
        }
        else if (Input.IsActionPressed("up"))
        {
            _animatedSprite2D.Play("up");
            _weaponHitDetector.TargetPosition = new Vector2(0, -20);
        }
        else if (Input.IsActionPressed("down"))
        {
            _animatedSprite2D.Play("down");
            _weaponHitDetector.TargetPosition = new Vector2(0, 20);
        }
        else
        {
            _animatedSprite2D.Play("stationary");
        }
    }

    private void Move()
    {
        Velocity = Input.GetVector("left", "right", "up", "down") * Speed;
        MoveAndSlide();
    }

    public int TryPickUp(IItem item, int count)
    {
        return _inventory.AddItem(item, count);
    }
}