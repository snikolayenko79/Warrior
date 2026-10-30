using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

public abstract class Container : MonoBehaviour, IContainer
{
    private List<IContainerItem> _items = new List<IContainerItem>();
    
    public IReadOnlyList<IContainerItem> Items => _items;

    public virtual void Initialize(IReadOnlyList<IContainerItem> items)
    {
        if (items == null)
            return;
        
        foreach (var item in items)
            AddItemToContainer(item);
    }
    
    public void AddItemToContainer(IContainerItem item)
    {
        _items.Add(item);
    }
    
    public void RemoveItemFromContainer(IContainerItem item)
    {
        _items.Remove(item);
    }
    
    public virtual void OnInteract(IContainerItem item) {}
}