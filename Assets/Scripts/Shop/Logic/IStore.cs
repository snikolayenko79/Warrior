using UnityEngine;
using System.Collections.Generic;

public interface IStore
{
    // Список товаров, которые сейчас есть на витрине
    IReadOnlyList<IPurchasable> Products { get; }
    
    // Метод совершения покупки
    void OnPurchase(ICustomer customer, IPurchasable item);
}