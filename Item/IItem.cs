using Godot;
using Kenting.Entity;

namespace KentingStation.Item;

// TODO To Consider: Add ItemId enum to replace typeof(item) calls 
public interface IItem
{
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