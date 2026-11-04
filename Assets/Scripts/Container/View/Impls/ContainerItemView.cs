using UnityEngine;

public class ContainerItemView : MonoBehaviour, IContainerItemView
{
    private IContainerItem _containerItem;
    public IContainerItem ItemData
    {
        get => _containerItem;
        set => _containerItem = value;
    }
    
    [SerializeField]private ContainerInteractLogic interactLogic;
    
    public IContainerInteractLogic InteractLogic => interactLogic;
}