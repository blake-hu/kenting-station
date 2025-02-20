using System;
using System.Text;
using KentingStation.Exception;
using KentingStation.Item;
using KentingStation.UI;

namespace Kenting.Common;

public record struct InventoryButtonId(int Row, int Col);

public class Inventory
{
    private InventoryButtonId _activeButton = new(0, 0);

    private InventoryButton[,] _buttonArray;
    // TODO: Add cache of item positions to allow us to add items to existing item stacks faster
    // TODO: Add priority queue of free inventory slots to allows us to add new items faster

    // TODO: Write algorithm for adding an item to inventory using above two data structures
    public bool AddItem(IItem item, int count)
    {
        throw new NotImplementedException();
    }

    public void RegisterButtonArray(InventoryButton[,] newButtonArray)
    {
        if (_buttonArray is not null)
            throw new KsReregistrationException(nameof(_buttonArray));
        if (newButtonArray.GetLength(0) == 0 || newButtonArray.GetLength(1) == 0)
            throw new Exception(
                $"newButtonArray has invalid dimensions [{newButtonArray.GetLength(0)},{newButtonArray.GetLength(1)}].");
        _buttonArray = newButtonArray;

        // DEBUG
        _buttonArray[0, 0].SetItem(ItemProvider.Singleton.Get<Beef>()); // Icon should be an egg
        _buttonArray[0, 1].SetItem(ItemProvider.Singleton.Get<Sword>()); // Icon should be a chicken coop
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
            throw new Exception($"{nameof(_buttonArray)} has not been instantiated yet.");
        for (var r = 0; r < _buttonArray.GetLength(0); r++)
        for (var c = 0; c < _buttonArray.GetLength(1); c++)
            if (_buttonArray[r, c].IsActive)
                sb.AppendLine($"InventoryButton[{r}, {c}]");
        return sb.ToString();
    }
}