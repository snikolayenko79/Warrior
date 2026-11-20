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
                int count = Load(inventoryItem);
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        _inventoryItems.Add(inventoryItem);
                        _items.Add(item);
                    }
                }
            }
        }
    }

    public void AddItemToInventory(IInventoryItem inventoryItem)
    {
        _inventoryItems.Add(inventoryItem);
        Save(inventoryItem, Mathf.Max(1, Load(inventoryItem) + 1));
        
        if (inventoryItem is IContainerItem containerItem)
            AddItemToContainer(containerItem);
    }

    public void RemoveItemFromInventory(IInventoryItem inventoryItem)
    {
        _inventoryItems.Remove(inventoryItem);
        Save(inventoryItem, Mathf.Max(0, Load(inventoryItem) - 1));
        
        if (inventoryItem is IContainerItem containerItem)
            RemoveItemFromContainer(containerItem);
    }

    public bool ContainsItem(IInventoryItem inventoryItem)
    {
        return _inventoryItems.Contains(inventoryItem);
    }

    public int ItemsCount => _inventoryItems.Count;
    public int MaxItemsCount => _config.MaxItemsCount;

    public IInventoryOwner Owner
    {
        get => _owner;
        set => _owner = value;
    }

    public int Load(IInventoryItem inventoryItem)
    {
        return Profile.GetVar<int>(inventoryItem.Name + "_collected", 0);
    }

    public void Save(IInventoryItem inventoryItem, int count)
    {
        Profile.SetVar<int>(inventoryItem.Name + "_collected", count);
    }
}