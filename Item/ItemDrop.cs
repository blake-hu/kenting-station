using Godot;
using Kenting.Entity;

namespace KentingStation.Item;

public partial class ItemDrop : Area2D
{
    private CollisionShape2D _collisionShape;
    private IItem _item;
    private int _itemCount;
    private Sprite2D _sprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (_item is null)
            throw new System.Exception($"{nameof(ItemDrop)}: {nameof(_item)} field is null.");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _sprite.Texture = _item.GetDisplayTexture();

        BodyEntered += OnBodyEntered;
        Monitoring = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player) return;
        var overflow = player.TryPickUp(_item, _itemCount);
        if (overflow == 0)
            QueueFree();
        else
            _itemCount = overflow;
    }

    public void SetItem(IItem item, int count)
    {
        _item = item;
        _itemCount = count;
    }
}