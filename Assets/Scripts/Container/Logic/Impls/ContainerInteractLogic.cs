using UnityEngine;

public abstract class ContainerInteractLogic : MonoBehaviour, IContainerInteractLogic
{
    public abstract void Execute(IContainer containerData, IContainerItem itemData);
}