using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Inventory/Item")]

public class InventoryItem : ContainerItem, IInventoryItem
{
    [Tooltip("Может ли быть несколько таких предметов в инвентаре одновременно")]
    [SerializeField]private bool isStackable;
    
    public bool IsStackable => isStackable;
}