using UnityEngine;

public interface IContainerItemView
{
    IContainerItem ItemData { get; set; }
    void OnInteract(IContainer containerData, IContainerItem itemData);
}