using KentingStation.Item;

namespace Kenting.Common;

// Counter that acts like a jar with a max capacity
// Min capacity assumed to be 0 (empty jar)
public class JarCounter(int maxCapacity)
{
    public JarCounter(IItem itemToCount) : this(itemToCount.MaxCountPerStack())
    {
    }

    public int MaxCapacity { get; } = maxCapacity;

    public int Count { get; private set; } // only allow set using Add/Remove methods

    public bool Add(int amount, out int overflowAmount)
    {
        var remainingSpace = MaxCapacity - Count;
        if (remainingSpace < amount)
        {
            overflowAmount = remainingSpace;
            Count = MaxCapacity;
            return true;
        }

        Count += amount;
        overflowAmount = 0;
        return true;
    }

    public bool Remove(int amount)
    {
        if (amount > Count)
            return false; // operation fails if trying to decrease Count below 0
        Count -= amount;
        return true;
    }
}