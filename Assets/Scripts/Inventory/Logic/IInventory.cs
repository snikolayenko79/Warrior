using UnityEngine;

public interface IInventory
{
    void OnUse(IInventoryOwner inventoryOwner, IInventoryItem inventoryItem);
}