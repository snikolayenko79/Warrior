using UnityEngine;

public abstract class InventoryItemView : ContainerItemView, IInventoryItemView
{
    public override void OnInteract(IContainer containerData, IContainerItem itemData)
    {
        if (containerData is IInventory inventoryData && itemData is IInventoryItem inventoryItemData)
            OnUse(inventoryData, inventoryItemData);
    }
    
    public abstract void OnUse(IInventory inventoryData, IInventoryItem itemData);
}