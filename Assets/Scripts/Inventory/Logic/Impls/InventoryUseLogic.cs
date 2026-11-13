using UnityEngine;

public abstract class InventoryUseLogic : ContainerInteractLogic, IInventoryUseLogic
{
    public abstract bool Execute(IInventory inventoryData, IInventoryItem itemData);
    
    public override void Execute(IContainer containerData, IContainerItem itemData)
    {
        if (containerData is IInventory inventoryData && itemData is IInventoryItem inventoryItemData)
        {
            if (Execute(inventoryData, inventoryItemData))
            {
                //TODO. проверить, если итем одноразовый, то удалять из инвентаря. Не забыть убрать удаление из InventoryUseLogic_Heal.
            }
        }
    }
}