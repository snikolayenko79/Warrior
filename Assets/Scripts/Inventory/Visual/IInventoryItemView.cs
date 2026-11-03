using UnityEngine;

public interface IInventoryItemView
{
    void OnUse(IInventory inventoryData, IInventoryItem itemData);
}