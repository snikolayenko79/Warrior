using UnityEngine;

public interface IContainerItem
{
    string Name { get; }
    Sprite Icon { get; }
    
    void OnInteract(IContainerInteractor interactor);
}