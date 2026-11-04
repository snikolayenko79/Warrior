using UnityEngine;
using UnityEngine.Serialization;

public abstract class ContainerItem: ScriptableObject, IContainerItem
{
    [SerializeField]private string itemName;
    [SerializeField]private ContainerItemView itemViewPrefab;
    
    public Sprite icon;

    public string Name => itemName;
    public Sprite Icon => icon;
    public ContainerItemView ItemViewPrefab => itemViewPrefab;
}