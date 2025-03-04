using Godot;
using KentingStation.Interface;
using Player = KentingStation.Entity.Instance.Player;

namespace KentingStation.Item.Instance;

public class Sword : IItem
{
    private readonly Texture2D _texture;

    public Sword()
    {
        // TODO Low Priority: Find sword texture
        _texture = ResourceLoader.Load<CompressedTexture2D>("res://Asset/SproutLands/Objects/ChickenHouse.png");
    }

    public int MaxCountPerStack => 1;

    public ItemType ItemType => ItemType.Sword;


    public Texture2D GetDisplayTexture()
    {
        return _texture;
    }


    public bool LeftClick(Player player)
    {
        return true;
    }
}