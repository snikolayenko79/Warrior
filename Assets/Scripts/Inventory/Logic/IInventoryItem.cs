using UnityEngine;

public interface IInventoryItem
{
    string Name { get; }
    void OnUse(IInventoryOwner inventoryOwner);
}