using UnityEngine;

public interface IPurchasableView
{
    void OnPurchase(IStore storeData, IPurchasable itemData);
}