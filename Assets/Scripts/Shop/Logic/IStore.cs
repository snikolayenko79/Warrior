using UnityEngine;
using System.Collections.Generic;

public interface IStore
{
    // Список товаров, которые сейчас есть в магазине
    IReadOnlyList<IPurchasable> Products { get; }
    
    void AddItemToStore(IPurchasable purchasable);
    void RemoveItemFromStore(IPurchasable purchasable);

    ICustomer Customer { get; set; }
    
    void OnPurchase(ICustomer customer, IPurchasable item);
}