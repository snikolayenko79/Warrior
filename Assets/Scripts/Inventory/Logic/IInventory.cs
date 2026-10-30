using UnityEditor;
using UnityEngine;

public interface IInventory
{
    void AddItemToInventory(IInventoryItem inventoryItem);
    void RemoveItemFromInventory(IInventoryItem inventoryItem);
    void OnUse(IInventoryOwner inventoryOwner, IInventoryItem inventoryItem);
    bool Load(IInventoryItem inventoryItem);
    void Save(IInventoryItem inventoryItem, bool bInInventory);
}