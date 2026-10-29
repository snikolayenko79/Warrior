using UnityEngine;

public interface IPurchasable
{    
    int Price { get; }
    void OnPurchase(ICustomer customer);
}