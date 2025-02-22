using System.Text;
using KentingStation.Exception;
using KentingStation.Interface;
using KentingStation.UI;

namespace KentingStation.Common;

public record struct InventoryButtonId(int Row, int Col);

public class Inventory
{
    private InventoryButtonId _activeButton = new(0, 0);

    private InventoryButton[,] _buttonArray;
    // TODO: Add cache of item positions to allow us to add items to existing item stacks faster
    // TODO: Add priority queue of free inventory slots to allows us to add new items faster

    // TODO: Write algorithm for adding an item to inventory using above two data structures
    public int AddItem(IItem item, int count)
    {
        // Basic algorithm that puts item in first free slot
        // Does not attempt to stack items
        for (var r = 0; r < _buttonArray.GetLength(0); r++)
        for (var c = 0; c < _buttonArray.GetLength(1); c++)
            if (_buttonArray[r, c].Count == 0)
            {
                _buttonArray[r, c].SetItem(item, count, out var overflow);
                return overflow;
            }

        return count; // No free slots to add item
    }

    public void RegisterButtonArray(InventoryButton[,] newButtonArray)
    {
        if (_buttonArray is not null)
            throw new KsReregistrationException(nameof(_buttonArray));
        if (newButtonArray.GetLength(0) == 0 || newButtonArray.GetLength(1) == 0)
            throw new System.Exception(
                $"newButtonArray has invalid dimensions [{newButtonArray.GetLength(0)},{newButtonArray.GetLength(1)}].");
        _buttonArray = newButtonArray;
    }

    public void UpdateActiveButton(InventoryButtonId newActiveButtonId)
    {
        // set previous active button to Inactive
        SetButtonState(_activeButton, false);
        _activeButton = newActiveButtonId;
        // update active button to latest
        SetButtonState(newActiveButtonId, true);
    }

    private void SetButtonState(InventoryButtonId buttonId, bool isActive)
    {
        _buttonArray[buttonId.Row, buttonId.Col].IsActive = isActive;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine("Active buttons: ");
        if (_buttonArray is null)
            throw new System.Exception($"{nameof(_buttonArray)} has not been instantiated yet.");
        for (var r = 0; r < _buttonArray.GetLength(0); r++)
        for (var c = 0; c < _buttonArray.GetLength(1); c++)
            if (_buttonArray[r, c].IsActive)
                sb.AppendLine($"InventoryButton[{r}, {c}]");
        return sb.ToString();
    }
}