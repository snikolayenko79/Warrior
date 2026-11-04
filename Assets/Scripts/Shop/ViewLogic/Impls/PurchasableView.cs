using UnityEngine;

public abstract class PurchasableView: ContainerItemView, IPurchasableView
{
    public override void OnInteract(IContainer containerData, IContainerItem itemData)
    {
        if (containerData is IStore storeData && itemData is IPurchasable storeItemData)
            OnPurchase(storeData, storeItemData);
    }
    
    public abstract void OnPurchase(IStore storeData, IPurchasable itemData);
}