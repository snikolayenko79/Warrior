using UnityEngine;

public class Inventory : Container, IInventory
{
    public override void OnInteract(IContainerInteractor interactor, IContainerItem item)
    {
        Debug.Log("1");
        if (interactor is IInventoryOwner inventoryOwner && item is IInventoryItem inventoryItem)
        {
            OnUse(inventoryOwner, inventoryItem);
        }
    }
        
    public void OnUse(IInventoryOwner inventoryOwner, IInventoryItem inventoryItem)
    {
        Debug.Log("2");
    }
}