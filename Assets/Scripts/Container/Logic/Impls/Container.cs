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
        OnItemAdded(item);
    }
    
    public void RemoveItemFromContainer(IContainerItem item)
    {
        _items.Remove(item);
        OnItemRemoved(item);
    }

    protected virtual void OnItemAdded(IContainerItem item) {}
    protected virtual void OnItemRemoved(IContainerItem item) {}
    
    public virtual void OnInteract(IContainerInteractor interactor, IContainerItem item) {}

    protected void Awake() {}
    protected void Start() {}
}