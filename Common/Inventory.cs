using Kenting.Interface;

namespace Kenting.Common;

public class Inventory
{
    private int _activeRow;
    private IInventoryItem[,] _itemArray;

    public Inventory(int numRows, int numCols)
    {
        _itemArray = new IInventoryItem[numRows, numCols];
    }
}