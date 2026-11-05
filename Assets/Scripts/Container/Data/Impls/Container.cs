using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

public abstract class Container : IContainer
{
    protected List<IContainerItem> _items = new List<IContainerItem>();
    
    public IReadOnlyList<IContainerItem> Items => _items;
    
    public event Action<IContainerItem> OnItemAdded;
    public event Action<IContainerItem> OnItemRemoved;

    public virtual void Initialize(IReadOnlyList<IContainerItem> items)
    {
        if (items == null)
            return;
        
        foreach (var item in items)
            _items.Add(item);
    }
    
    public void AddItemToContainer(IContainerItem item)
    {
        _items.Add(item);
        OnItemAdded?.Invoke(item);
    }
    
    public void RemoveItemFromContainer(IContainerItem item)
    {
        _items.Remove(item);
        OnItemRemoved?.Invoke(item);
    }
}