using Kenting.Entity;

namespace KentingStation.Item;

public interface IItem
{
    public int MaxCountPerStack();

    public bool LeftClick(Player player)
    {
        return true; // default behavior: clicking has no effect
    }

    public bool RightClick(Player player)
    {
        return true; // default behavior: clicking has no effect
    }
}