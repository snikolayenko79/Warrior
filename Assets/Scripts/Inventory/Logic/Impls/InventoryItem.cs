using UnityEngine;

public abstract class InventoryItem : ContainerItem, IInventoryItem
{ 
     public abstract void OnUse(IInventoryOwner inventoryOwner);
}