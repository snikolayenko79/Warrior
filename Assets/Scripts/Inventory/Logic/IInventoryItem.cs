using UnityEngine;

public interface IInventoryItem
{
    void OnUse(IInventoryOwner inventoryOwner);
}