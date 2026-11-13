using UnityEngine;

public interface IInventoryUseLogic
{
    bool Execute(IInventory inventoryData, IInventoryItem itemData);
}