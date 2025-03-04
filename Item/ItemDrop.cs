using Godot;
using KentingStation.Interface;
using Player = KentingStation.Entity.Instance.Player;

namespace KentingStation.Item;

public partial class ItemDrop : Area2D
{
    private IItem _item;
    private int _itemCount;
    private Sprite2D _sprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (_item is null)
            throw new System.Exception($"{nameof(ItemDrop)}: {nameof(_item)} field is null.");
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _sprite.Texture = _item.GetDisplayTexture();

        BodyEntered += OnBodyEntered;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            var overflow = player.TryPickUp(_item, _itemCount);
            if (overflow == 0)
                QueueFree();
            else
                _itemCount = overflow;
        }
        else if (body is IPredatorPreyEntity predPreyEntity &&
                 _item.EntitiesThatCanPickUp().Contains(body.GetType()))
        {
            _item.PickedUpBy(predPreyEntity);
            QueueFree();
        }
    }

    public void SetItem(IItem item, int count)
    {
        _item = item;
        _itemCount = count;
    }
}