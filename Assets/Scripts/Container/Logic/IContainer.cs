using UnityEngine;
using System.Collections.Generic;

public interface IContainer
{
    // Список итемов, которые сейчас есть в контейнере
    IReadOnlyList<IContainerItem> Items { get; }
    
    void Initialize(IReadOnlyList<IContainerItem> items);
    
    void AddItemToContainer(IContainerItem item);
    void RemoveItemFromContainer(IContainerItem item);
}