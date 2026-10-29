using UnityEngine;
using System.Collections.Generic;

public abstract class Container : MonoBehaviour, IContainer
{
    private List<IContainerItem> _items = new List<IContainerItem>();

    public IReadOnlyList<IContainerItem> Items => _items;
    
    public abstract void Interact(IContainerInteractor interactor, IContainerItem item);
}