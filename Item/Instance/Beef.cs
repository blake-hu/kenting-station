using Godot;
using KentingStation.Interface;
using Player = KentingStation.Entity.Instance.Player;

namespace KentingStation.Item.Instance;

public class Beef : IItem
{
    private readonly Texture2D _texture;

    public Beef() // Do not construct directly, use ItemProvider instead
    {
        // TODO Low Priority: Find beef texture
        _texture = ResourceLoader.Load<CompressedTexture2D>("res://Asset/SproutLands/Objects/EggItem.png");
    }

    public ItemType ItemType()
    {
        return Item.ItemType.Beef;
    }

    public int MaxCountPerStack()
    {
        return 64;
    }

    public Texture2D GetDisplayTexture()
    {
        return _texture;
    }

    public bool RightClick(Player player)
    {
        return true;
    }
}