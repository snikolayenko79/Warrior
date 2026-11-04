using UnityEngine;

public abstract class PurchaseLogic : ContainerInteractLogic, IPurchaseLogic
{
    public abstract void Execute(IStore storeData, IPurchasable itemData);

    public override void Execute(IContainer containerData, IContainerItem itemData)
    {
        if (containerData is IStore storeData && itemData is IPurchasable storeItemData)
            Execute(storeData, storeItemData);
    }
}