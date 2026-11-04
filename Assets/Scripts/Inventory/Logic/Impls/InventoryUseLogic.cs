using UnityEngine;

public abstract class InventoryUseLogic : ContainerInteractLogic, IInventoryUseLogic
{
    public abstract void Execute(IInventory inventoryData, IInventoryItem itemData);
    
    public override void Execute(IContainer containerData, IContainerItem itemData)
    {
        if (containerData is IInventory inventoryData && itemData is IInventoryItem inventoryItemData)
            Execute(inventoryData, inventoryItemData);
    }
}