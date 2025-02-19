using Godot;
using Kenting.Entity;

namespace KentingStation.Item;

public class Sword : IItem
{
    public int MaxCountPerStack()
    {
        return 1;
    }

    public bool LeftClick(Player player)
    {
        GD.Print("Swing sword!");
        return true;
    }
}