using Godot;
using Kenting.Entity;

namespace KentingStation.Item;

public interface IItem
{
    public ItemType ItemType();

    public int MaxCountPerStack();

    public Texture2D GetDisplayTexture();

    public bool LeftClick(Player player)
    {
        return true; // default behavior: clicking has no effect
    }

    public bool RightClick(Player player)
    {
        return true; // default behavior: clicking has no effect
    }
}