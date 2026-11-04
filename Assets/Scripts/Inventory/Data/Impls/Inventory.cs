using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class Inventory : Container, IInventory
{
    private List<IInventoryItem> _inventoryItems = new List<IInventoryItem>();
    
    public IReadOnlyList<IInventoryItem> InventoryItems => _inventoryItems;
    
    private InventoryConfig _config;
    private IInventoryOwner _owner;

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
                {
                    _inventoryItems.Add(inventoryItem);
                    _items.Add(item);
                }
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

    public bool ContainsItem(IInventoryItem inventoryItem)
    {
        return _inventoryItems.Contains(inventoryItem);
    }

    public IInventoryOwner Owner
    {
        get => _owner;
        set => _owner = value;
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