using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class Store : MonoBehaviour, IStore
{
    private List<IPurchasable> _products = new List<IPurchasable>();
    
    public IReadOnlyList<IPurchasable> Products => _products;

    [Inject]
    public void Construct(List<IPurchasable> products)
    {
        _products.Clear();
        _products.AddRange(products);
    }
    
    public void Purchase(ICustomer customer, IPurchasable item)
    {
        if (customer.CanAfford(item.Price))
        {
            customer.DeductCoins(item.Price);
            item.OnPurchase(customer);
        }
    }
}