using UnityEngine;

public abstract class ContainerItemView : MonoBehaviour, IContainerItemView
{
    private IContainerItem _containerItemDataData;
    public IContainerItem ItemData
    {
        get => _containerItemDataData;
        set => _containerItemDataData = value;
    }

    public abstract void OnInteract(IContainer containerData, IContainerItem itemData);
}