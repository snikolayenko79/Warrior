using UnityEngine;

public abstract class StoreItem : ContainerItem, IPurchasable
{
    public int price;
    public int Price => price;
    
    public abstract void OnPurchase(ICustomer customer);
}