using UnityEngine;

public abstract class PurchaseLogic : ContainerInteractLogic, IPurchaseLogic
{
    public abstract bool Execute(IStore storeData, IPurchasable itemData);

    public override void Execute(IContainer containerData, IContainerItem itemData)
    {
        if (containerData is IStore storeData && itemData is IPurchasable storeItemData)
        {
            if (storeData.Customer.Coins >= storeItemData.Price)
            {
                if (Execute(storeData, storeItemData))
                {
                    storeData.Customer.Coins -= storeItemData.Price;
                }
            }
        }
    }
}