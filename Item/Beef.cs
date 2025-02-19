using Godot;
using Kenting.Entity;

namespace KentingStation.Item;

public class Beef : IItem
{
    public int MaxCountPerStack()
    {
        return 64;
    }

    public bool RightClick(Player player)
    {
        GD.Print("Ate beef!");
        return true;
    }
}