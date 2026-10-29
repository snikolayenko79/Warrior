using UnityEngine;
using UnityEngine.Serialization;

public abstract class ContainerItem: ScriptableObject, IContainerItem
{
    [FormerlySerializedAs("productName")] public string itemName;
    public Sprite icon;

    public string Name => itemName;
    public Sprite Icon => icon;
    
    public abstract void OnInteract(IContainerInteractor interactor);
}