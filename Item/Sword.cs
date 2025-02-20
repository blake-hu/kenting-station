using Godot;
using Kenting.Entity;

namespace KentingStation.Item;

public class Sword : IItem
{
    private readonly ImageTexture _texture;

    public Sword()
    {
        // TODO Low Priority: Find sword texture
        var image = Image.LoadFromFile("res://Asset/SproutLands/Objects/ChickenHouse.png");
        _texture = ImageTexture.CreateFromImage(image);
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