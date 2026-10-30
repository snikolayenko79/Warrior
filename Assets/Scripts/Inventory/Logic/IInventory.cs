using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public interface IInventory
{
    // Список предметов, которые сейчас есть в инвентаре
    IReadOnlyList<IInventoryItem> InventoryItems { get; }
    
    void AddItemToInventory(IInventoryItem inventoryItem);
    void RemoveItemFromInventory(IInventoryItem inventoryItem);
    
    IInventoryOwner Owner { get; set; }
    
    void OnUse(IInventoryOwner inventoryOwner, IInventoryItem inventoryItem);
    
    bool Load(IInventoryItem inventoryItem);
    void Save(IInventoryItem inventoryItem, bool bInInventory);
}