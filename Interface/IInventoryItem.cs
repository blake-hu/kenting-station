using Kenting.Entity;

namespace Kenting.Interface;

public interface IInventoryItem
{
    public bool LeftClick(Player player);

    public bool RightClick(Player player);

    public int MaxCount();

    public bool IncreaseCount();

    public bool DecreaseCount();
}