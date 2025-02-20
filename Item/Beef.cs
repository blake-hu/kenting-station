using Godot;
using Kenting.Entity;

namespace KentingStation.Item;

public class Beef : IItem
{
    private readonly Texture2D _texture;

    public Beef()
    {
        // TODO Low Priority: Find beef texture
        var image = Image.LoadFromFile("res://Asset/SproutLands/Objects/EggItem.png");
        _texture = ImageTexture.CreateFromImage(image);
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
        GD.Print("Ate beef!");
        return true;
    }
}