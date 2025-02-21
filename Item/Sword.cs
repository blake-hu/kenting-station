using Godot;
using Kenting.Entity;

namespace KentingStation.Item;

public class Sword : IItem
{
    private readonly Texture2D _texture;

    public Sword()
    {
        // TODO Low Priority: Find sword texture
        _texture = ResourceLoader.Load<CompressedTexture2D>("res://Asset/SproutLands/Objects/ChickenHouse.png");
    }

    public int MaxCountPerStack()
    {
        return 1;
    }

    public Texture2D GetDisplayTexture()
    {
        return _texture;
    }

    public bool LeftClick(Player player)
    {
        GD.Print("Swing sword!");
        return true;
    }
}