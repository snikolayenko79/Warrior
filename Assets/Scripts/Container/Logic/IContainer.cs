using UnityEngine;
using System.Collections.Generic;

public interface IContainer
{
    // Список итемов, которые сейчас есть в контейнере
    IReadOnlyList<IContainerItem> Items { get; }
    
    // Метод совершения действия с айтемами из контейнера
    void OnInteract(IContainerInteractor interactor, IContainerItem item);
}