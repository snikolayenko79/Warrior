using UnityEngine;

public interface IPurchasable
{
    string Name { get; }
    int Price { get; }
    Sprite Icon { get; }
    
    void OnPurchase(ICustomer customer);
}