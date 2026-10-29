using UnityEngine;
using System.Collections.Generic;
using Zenject;

public abstract class Container : MonoBehaviour, IContainer
{
    private List<IContainerItem> _items = new List<IContainerItem>();

    public IReadOnlyList<IContainerItem> Items => _items;
    
    public abstract void Interact(IContainerInteractor interactor, IContainerItem item);
    
    [Inject]
    public void Construct(ContainerConfig config)
    {
        _items.Clear();
        _items.AddRange(config.Items);
    }
}