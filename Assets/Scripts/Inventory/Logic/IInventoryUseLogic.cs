using UnityEngine;

public interface IInventoryUseLogic
{
    void Execute(IInventory inventoryData, IInventoryItem itemData);
}