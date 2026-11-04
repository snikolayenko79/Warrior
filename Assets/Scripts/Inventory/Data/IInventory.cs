using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public interface IInventory
{
    // Список предметов, которые сейчас есть в инвентаре
    IReadOnlyList<IInventoryItem> InventoryItems { get; }
    
    void AddItemToInventory(IInventoryItem inventoryItem);
    void RemoveItemFromInventory(IInventoryItem inventoryItem);
    
    bool ContainsItem(IInventoryItem inventoryItem);
    
    IInventoryOwner Owner { get; set; }
    
    bool Load(IInventoryItem inventoryItem);
    void Save(IInventoryItem inventoryItem, bool bInInventory);
}