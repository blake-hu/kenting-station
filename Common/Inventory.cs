using System;
using KentingStation.Item;

namespace Kenting.Common;

public struct InventoryItem(IItem item, JarCounter counter)
{
    public IItem Item = item;
    public JarCounter Counter = counter;
}

public class Inventory
{
    private readonly InventoryItem[,] _itemArray;
    private int _activeRow;

    public Inventory(int numRows, int numCols)
    {
        if (numRows <= 0) throw new ArgumentOutOfRangeException(nameof(numRows));
        if (numCols <= 0) throw new ArgumentOutOfRangeException(nameof(numCols));
        _itemArray = new InventoryItem[numRows, numCols];
        _activeRow = 0;
    }
}