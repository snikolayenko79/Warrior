using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class Inventory : Container, IInventory
{
    private List<IInventoryItem> _inventoryItems = new List<IInventoryItem>();
    
    public IReadOnlyList<IInventoryItem> InventoryInventoryItemsInventoryItems => _inventoryItems;
    
    private InventoryConfig _config;

    [Inject]
    public void Construct(InventoryConfig config)
    {
        _config = config;
        Initialize(config.Items);
    }

    public override void Initialize(IReadOnlyList<IContainerItem> items)
    {
        if (items == null)
            return;
        
        foreach (var item in items)
        {
            if (item is IInventoryItem inventoryItem)
            {
                if (Load(inventoryItem))
                    AddItemToInventory(inventoryItem);
            }
        }
    }

    public void AddItemToInventory(IInventoryItem inventoryItem)
    {
        _inventoryItems.Add(inventoryItem);
        Save(inventoryItem, true);
        
        if (inventoryItem is IContainerItem containerItem)
            AddItemToContainer(containerItem);
    }

    public void RemoveItemFromInventory(IInventoryItem inventoryItem)
    {
        _inventoryItems.Remove(inventoryItem);
        Save(inventoryItem, false);
        
        if (inventoryItem is IContainerItem containerItem)
            RemoveItemFromContainer(containerItem);
    }
    
    public override void OnInteract(IContainerInteractor interactor, IContainerItem item)
    {
        if (interactor is IInventoryOwner inventoryOwner && item is IInventoryItem inventoryItem)
        {
            OnUse(inventoryOwner, inventoryItem);
        }
    }
        
    public void OnUse(IInventoryOwner inventoryOwner, IInventoryItem inventoryItem)
    {
        
    }

    public bool Load(IInventoryItem inventoryItem)
    {
        // TODO. Load item state: in inventory or not.
        return false;
    }

    public void Save(IInventoryItem inventoryItem, bool bInInventory)
    {
        // TODO. Save item state: in inventory or not.
    }
}